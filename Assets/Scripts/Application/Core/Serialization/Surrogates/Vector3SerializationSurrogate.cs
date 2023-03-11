namespace Application.Core.Serialization.Surrogates
{
    using System.Runtime.Serialization;
    using UnityEngine;

    /// <summary>
    /// Defines how to serialize UnityEngine vector3s.
    /// </summary>
    public class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        /// <inheritdoc/>
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 v3 = (Vector3)obj;

            if (info != null)
            {
                info.AddValue("x", v3.x);
                info.AddValue("y", v3.y);
                info.AddValue("z", v3.z);
            }
        }

        /// <inheritdoc/>
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 v3 = (Vector3)obj;

            if (info != null)
            {
                v3.x = (float)info.GetValue("x", typeof(float));
                v3.y = (float)info.GetValue("y", typeof(float));
                v3.z = (float)info.GetValue("z", typeof(float));
            }

            return v3;
        }
    }
}
