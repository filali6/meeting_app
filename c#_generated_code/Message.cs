using System;

public class Message {
	public string SourceUserId;
	public string TargetUserId;
	public string Content;
	public DateTime ReadDate;
	public DateTime SentDate;
	public bool SourceDeleted;
	public bool TargetDeleted;

	private AppUser send;

	private AppUser receive;

}
