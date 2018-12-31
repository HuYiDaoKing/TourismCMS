using System.IO;

namespace OneTrip.Common.Utility
{
  public static class FileHelper
  {
    public static string GuidFileName(string originalFileName)
    {
      return System.Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(originalFileName);
    }
  }
}