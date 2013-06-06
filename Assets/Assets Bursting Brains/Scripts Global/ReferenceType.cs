class ReferenceType<T> where T : struct
{
    public T Value { get; set; }
    public ReferenceType(T value) { this.Value = value; }
}
