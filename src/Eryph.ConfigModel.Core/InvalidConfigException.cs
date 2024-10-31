using System;

namespace Eryph.ConfigModel;

/// <summary>
/// This exception is thrown when a configuration cannot be deserialized.
/// </summary>
public class InvalidConfigException(string message, Exception ex)
    : Exception(message, ex);