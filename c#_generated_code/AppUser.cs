using System;

public class AppUser {
	public string UserName;
	public DateTime DateBirth;
	public string KnownAs;
	public DateTime Created;
	public DateTime LastActive;
	public bool IsMale;
	public string Introduction;
	public string Interests;
	public string LookingFor;
	public string City;
	public string Country;

	private Like[] receive;

	private Photo[] add;
	private Like[] give;
	private Message[] send;
	private UserRole userRole;

}
