using System;
using System.Collections;


static public class Utils
{

	private static readonly DateTime UnixEpoch =
	    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static long GetCurrentUnixTimestampMillis()
	{
	    return (long) (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
	}

}
