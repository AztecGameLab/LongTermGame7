namespace Application.Core.Serialization.Surrogates
{
    using System.Runtime.Serialization;
    using UnityEngine;

    /// <summary>
    /// Defines how to serialize a quaternion.
    /// </summary>
    public class QuaternionSerializationSurrogate : ISerializationSurrogate
    {
        /// <inheritdoc/>
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quaternion q = (Quaternion)obj;

            if (info != null)
            {
                info.AddValue("w", q.w);
                info.AddValue("x", q.x);
                info.AddValue("y", q.y);
                info.AddValue("z", q.z);
            }
        }

        /// <inheritdoc/>
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion q = (Quaternion)obj;

            if (info != null)
            {
                q.w = (float)info.GetValue("w", typeof(float));
                q.x = (float)info.GetValue("x", typeof(float));
                q.y = (float)info.GetValue("y", typeof(float));
                q.z = (float)info.GetValue("z", typeof(float));
            }

            return q;
        }
    }
}
