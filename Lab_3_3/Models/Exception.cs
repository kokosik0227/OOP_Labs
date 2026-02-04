using System;
namespace Models
{
public class EntityException : Exception
{
public EntityException(string message, Exception? inner=null) : base(message, inner) { }
}
}