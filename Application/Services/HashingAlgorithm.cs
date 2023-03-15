using Domain.Services;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;
public class HashingAlgorithm : IHashingAlgorithm
{
	public string HashString(string clearText)
	{
		HMACSHA1 hash = new HMACSHA1();
		hash.Key = HexToByte("0D6EA6BB2E842730BE1C6B1478BAAA161DB51E0DFF7EBB7F8D71905DC1D4BCA83631C34E06B7E3F43BFAF8AFF31C1B8221B08F6200FB4C87D2A2C7A78F91462C"); //For DBC
		return Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(clearText)));
	}

	private byte[] HexToByte(string hexString)
	{
		byte[] returnBytes = new byte[hexString.Length / 2];
		for (int i = 0; i < returnBytes.Length; i++)
			returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
		return returnBytes;
	}
}
