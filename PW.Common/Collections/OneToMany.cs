using System.Collections.Generic;

namespace PW.Collections;

#if NET5_0_OR_GREATER

/// <summary>
/// Represents a one-to-many associative grouping where 'One' and 'Many' are of different types.
/// </summary>
public record ReadonlyOneToMany<TOne, TMany>(TOne One, IReadOnlyList<TMany> Many);

/// <summary>
/// Represents a one-to-many associative grouping where 'One' and 'Many' are of different types.
/// </summary>
public record OneToMany<TOne, TMany>(TOne One, List<TMany> Many);

#endif

