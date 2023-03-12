namespace Domain.Services;

public interface IHashingAlgorithm
{
	string HashString(string clearText);
}
