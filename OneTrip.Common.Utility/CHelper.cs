using System;

namespace OneTrip.Common.Utility
{
  public class CHelper
  {
    #region 处理数据库中可空值类型 TODO:想办法封装

    /// <summary>
    /// 处理整形可空值类型
    /// </summary>
    /// <param name="strObj"></param>
    /// <returns></returns>
    public static int? NullableIntObj(string strObj)
    {
      //处理可空值类型的特殊字段
      int? obj;
      if (strObj == string.Empty)
        obj = null;
      else
        obj = int.Parse(strObj);

      return obj;
    }

    /// <summary>
    /// 处理日期型可空值类型
    /// </summary>
    /// <param name="strObj"></param>
    /// <returns></returns>
    public static DateTime? NullableDateObj(string strObj)
    {
      //处理可空值类型的特殊字段
      DateTime? obj;
      if (strObj == string.Empty)
        obj = null;
      else
        obj = DateTime.Parse(strObj);

      return obj;
    }

    /// <summary>
    /// 处理Double可空值类型
    /// </summary>
    /// <param name="strObj"></param>
    /// <returns></returns>
    public static double? NullableDoubleObj(string strObj)
    {
      //处理可空值类型的特殊字段
      double? obj;
      if (strObj == string.Empty)
        obj = null;
      else
        obj = double.Parse(strObj);

      return obj;
    }

    #endregion 处理数据库中可空值类型 TODO:想办法封装
  }
}