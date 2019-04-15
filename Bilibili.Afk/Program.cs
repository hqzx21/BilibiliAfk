using System;
using Bilibili.Api;
using Bilibili.Settings;
using System.Reflection;

namespace Bilibili.Afk {
	internal static class Program {
		private static void Main(string[] args) {
			UserList users;

			Console.Title = GetTitle();
			GlobalSettings.Logger = Logger.Instance;
			if (!BitConverter.IsLittleEndian) {
				GlobalSettings.Logger.LogWarning("在BigEndian模式的CPU下工作可能导致程序出错");
				GlobalSettings.Logger.LogWarning("如果出现错误，请创建issue");
				//throw new PlatformNotSupportedException();
			}
			try {
				GlobalSettings.LoadAll();
			}
			catch {
				GlobalSettings.Logger.LogError("缺失或无效配置文件，请检查是否添加\"Users.json\"");
				Console.ReadKey(true);
				return;
			}
			users = GlobalSettings.Users;
			User user = users[0];
			var result = user.Login().GetAwaiter().GetResult();
			Console.ReadKey(true);
		}

		public static string GetTitle() {
			string productName;
			string version;

			productName = GetAssemblyAttribute<AssemblyProductAttribute>().Product;
			version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			return $"{productName} v{version}";
		}

		private static T GetAssemblyAttribute<T>() {
			return (T)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false)[0];
		}
	}
}
