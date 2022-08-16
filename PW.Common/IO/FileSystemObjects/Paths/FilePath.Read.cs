using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.IO.FileSystemObjects;

public partial class FilePath
{

  public string ReadAllText() => File.ReadAllText(Path);

  public string[] ReadAllLines => File.ReadAllLines(Path);

  public byte[] ReadAllBytes => File.ReadAllBytes(Path);


}
