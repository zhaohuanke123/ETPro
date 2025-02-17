/* Copyright 2010-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Bson
{
    /// <summary>
    /// Represents a BSON document that is deserialized lazily.
    /// </summary>
    [BsonSerializer(typeof(LazyBsonDocumentSerializer))]
    public class LazyBsonDocument : MaterializedOnDemandBsonDocument
    {
        // private fields
        private IByteBuffer _slice;
        private List<IDisposable> _disposableItems = new List<IDisposable>();
        private BsonBinaryReaderSettings _readerSettings = BsonBinaryReaderSettings.Defaults;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBsonDocument"/> class.
        /// </summary>
        /// <param name="slice">The slice.</param>
        /// <exception cref="System.ArgumentNullException">slice</exception>
        /// <exception cref="System.ArgumentException">LazyBsonDocument cannot be used with an IByteBuffer that needs disposing.</exception>
        public LazyBsonDocument(IByteBuffer slice)
        {
            if (slice == null)
            {
                throw new ArgumentNullException("slice");
            }

            _slice = slice;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBsonDocument"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public LazyBsonDocument(byte[] bytes)
            : this(new ByteArrayBuffer(bytes, isReadOnly: true))
        {
        }

        // public properties
        /// <summary>
        /// Gets the slice.
        /// </summary>
        /// <value>
        /// The slice.
        /// </value>
        public IByteBuffer Slice
        {
            get { return _slice; }
        }

        /// <summary>
        /// Creates a shallow clone of the document (see also DeepClone).
        /// </summary>
        /// <returns>
        /// A shallow clone of the document.
        /// </returns>
        public override BsonValue Clone()
        {
            if (_slice != null)
            {
                return new LazyBsonDocument(CloneSlice());
            }
            else
            {
                return base.Clone();
            }
        }

        /// <summary>
        /// Creates a deep clone of the document (see also Clone).
        /// </summary>
        /// <returns>
        /// A deep clone of the document.
        /// </returns>
        public override BsonValue DeepClone()
        {
            if (_slice != null)
            {
                return new LazyBsonDocument(CloneSlice());
            }
            else
            {
                return base.DeepClone();
            }
        }

        // protected methods
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_slice != null)
                    {
                        _slice.Dispose();
                        _slice = null;
                    }
                    if (_disposableItems != null)
                    {
                        _disposableItems.ForEach(x => x.Dispose());
                        _disposableItems = null;
                    }
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Materializes the BsonDocument.
        /// </summary>
        /// <returns>The materialized elements.</returns>
        protected override IEnumerable<BsonElement> Materialize()
        {
            return MaterializeThisLevel();
        }

        /// <summary>
        /// Informs subclasses that the Materialize process completed so they can free any resources related to the unmaterialized state.
        /// </summary>
        protected override void MaterializeCompleted()
        {
            var slice = _slice;
            _slice = null;
            slice.Dispose();
        }

        // private methods
        private IByteBuffer CloneSlice()
        {
            return _slice.GetSlice(0, _slice.Length);
        }

        private LazyBsonArray DeserializeLazyBsonArray(BsonBinaryReader bsonReader)
        {
            var slice = bsonReader.ReadRawBsonArray();
            var nestedArray = new LazyBsonArray(slice);
            _disposableItems.Add(nestedArray);
            return nestedArray;
        }

        private LazyBsonDocument DeserializeLazyBsonDocument(BsonBinaryReader bsonReader)
        {
            var slice = bsonReader.ReadRawBsonDocument();
            var nestedDocument = new LazyBsonDocument(slice);
            _disposableItems.Add(nestedDocument);
            return nestedDocument;
        }

        private IEnumerable<BsonElement> MaterializeThisLevel()
        {
            var elements = new List<BsonElement>();

            using (var stream = new ByteBufferStream(_slice, ownsBuffer: false))
            using (var bsonReader = new BsonBinaryReader(stream, _readerSettings))
            {
                var context = BsonDeserializationContext.CreateRoot(bsonReader);

                bsonReader.ReadStartDocument();
                BsonType bsonType;
                while ((bsonType = bsonReader.ReadBsonType()) != BsonType.EndOfDocument)
                {
                    var name = bsonReader.ReadName();
                    BsonValue value;
                    switch (bsonType)
                    {
                        case BsonType.Array: value = DeserializeLazyBsonArray(bsonReader); break;
                        case BsonType.Document: value = DeserializeLazyBsonDocument(bsonReader); break;
                        default: value = BsonValueSerializer.Instance.Deserialize(context); break;
                    }
                    elements.Add(new BsonElement(name, value));
                }
                bsonReader.ReadEndDocument();
            }

            return elements;
        }
    }
}
