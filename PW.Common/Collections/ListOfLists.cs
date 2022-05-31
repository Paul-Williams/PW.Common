using System.Collections.Generic;

namespace PW.Collections;

/// <summary>
/// A list where each element is a <see cref="List{T}"/>
/// </summary>  
public class ListOfLists<T> : List<List<T>>{}
