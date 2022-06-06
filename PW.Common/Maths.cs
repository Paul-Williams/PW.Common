 

namespace PW;

/// <summary>
/// Some maths methods
/// </summary>
public static class Maths
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="num"></param>
  /// <returns></returns>
  public static bool IsPerfectSquare(int num) => num > 0 && (System.Math.Sqrt(num) % 1 == 0);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="num"></param>
  /// <returns></returns>
  public static (bool Success, int Root) GetPerfectRoot(int num)
  {
    if (num < 2) return (false, 0);
    var root = System.Math.Sqrt(num);
    return root % 1 == 0 ? (true, (int)root) : (false, 0);
  }
}
