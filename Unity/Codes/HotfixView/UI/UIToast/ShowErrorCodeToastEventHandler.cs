using ET.UIEventType;

namespace ET
{
	public class ShowErrorCodeToastEventHandler: AEventAsync<UIEventType.ShowErrorToast>
	{
		protected override async ETTask Run(ShowErrorToast a)
		{
			string text = null;
			switch (a.ErrorCode)
			{
				case ErrorCode.ERR_AccountOrPasswordError:
					text = "账号或密码错误，请重新输入!";
					break;

				case ErrorCode.ERR_LoginInfoEmpty:
					text = "账号或密码为空!";
					break;

				// 逻辑层错误
				case ErrorCode.ERR_AccountAlreadyRegister:
					text = "账号已注册，请更换账号或直接登录!";
					break;

				case ErrorCode.ERR_NotBindPlayer:
					text = "未绑定玩家信息，请重新登录!";
					break;

				// 网络相关错误
				case ErrorCode.ERR_NetWorkError:
					text = "网络错误，请检查您的网络连接!";
					break;

				case ErrorCode.ERR_LoginInfoIsNull:
					text = "登录信息错误，请检查账号和密码!";
					break;

				case ErrorCode.ERR_AccountNameFormError:
					text = "账号格式错误：\n账号必须包含至少一个大写字母、一个小写字母和一个数字，且长度在6到15个字符之间。";
					break;

				case ErrorCode.ERR_PasswordFormError:
					text = "密码格式错误，请输入正确的密码!";
					break;

				case ErrorCode.ERR_AccountInBlackListError:
					text = "账号已被加入黑名单，如有疑问请联系客服!";
					break;

				case ErrorCode.ERR_LoginPasswordError:
					text = "登录密码错误，请重新输入!";
					break;

				case ErrorCode.ERR_RequestRepeatedly:
					text = "请勿重复请求!";
					break;

				case ErrorCode.ERR_AccountNotExistError:
					text = "账号不存在!";
					break;

				case ErrorCode.GoldNotEnough:
					text = "金币不足!";
					break;

				default:
					text = $"未知错误: {a.ErrorCode}";
					break;
			}

			await EventSystem.Instance.PublishAsync(new ShowToast()
			{
				Scene = a.Scene,
				Text = text
			});
		}
	}
}
