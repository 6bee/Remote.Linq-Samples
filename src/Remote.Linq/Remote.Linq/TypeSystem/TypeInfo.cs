﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq.TypeSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract(Name = "Type")]
    public class TypeInfo
    {
        public TypeInfo()
        {
        }

        public TypeInfo(Type type)
        {
            if (ReferenceEquals(null, type))
            {
                throw new ArgumentNullException("type");
            }

            _type = type;

            Name = type.Name;

            Namespace = type.Namespace;

            if (type.IsNested && !type.IsGenericParameter)
            {
                DeclaringType = new TypeInfo(type.DeclaringType);
            }

            if (type.IsGenericType())
            {
                GenericArguments = type.GetGenericArguments().Select(x => new TypeInfo(x)).ToList();
            }

            IsAnonymousType = type.IsAnonymousType();

            if (IsAnonymousType)
            {
                Properties = type.GetProperties().Select(x => x.Name).ToList();
            }
        }

        [DataMember(Order = 1, IsRequired = true, EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Order = 2, IsRequired = false, EmitDefaultValue = false)]
        public string Namespace { get; set; }

        [DataMember(Order = 3, IsRequired = false, EmitDefaultValue = false)]
        public TypeInfo DeclaringType { get; set; }

        [DataMember(Order = 4, IsRequired = false, EmitDefaultValue = false)]
        public List<TypeInfo> GenericArguments { get; set; }

        [DataMember(Order = 5, IsRequired = false, EmitDefaultValue = false)]
        public bool IsAnonymousType { get; set; }

        [DataMember(Order = 6, IsRequired = false, EmitDefaultValue = false)]
        public List<string> Properties { get; set; }

        public bool IsNested { get { return !ReferenceEquals(null, DeclaringType); } }

        public bool IsGenericType { get { return !ReferenceEquals(null, GenericArguments) && GenericArguments.Any(); } }

        internal string FullName
        {
            get
            {
                if (IsNested)
                {
                    var fullname = string.Format("{0}+{1}",
                        DeclaringType.FullName,
                        Name);
                    return fullname;
                }
                else
                {
                    var fullname = string.Format("{0}{1}{2}",
                        Namespace,
                        string.IsNullOrEmpty(Namespace) ? null : ".",
                        Name);
                    return fullname;
                }
            }
        }

        /// <summary>
        /// Resolves this type info instance to it's type using the default type resolver instance.
        /// </summary>
        public Type Type
        {
            get
            {
                if (ReferenceEquals(null, _type))
                {
                    _type = TypeResolver.Instance.ResolveType(this);
                }
                return _type;
            }
        }
        [NonSerialized]
        private Type _type;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TypeInfo);
        }

        public bool Equals(TypeInfo typeInfo)
        {
            if (ReferenceEquals(null, typeInfo)) return false;
            if (ReferenceEquals(this, typeInfo)) return true;
            var s0 = ToString();
            var s1 = typeInfo.ToString();
            return string.Equals(s0, s1, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            var s = ToString();
            return s.GetHashCode();
        }

        public override string ToString()
        {
            var genericArguments = IsGenericType
                ? string.Format("[{0}]", string.Join(",", GenericArguments.Select(x => x.ToString()).ToArray()))
                : null;
            return string.Format("{0}{1}", FullName, genericArguments);
        }

        public static explicit operator Type(TypeInfo t)
        {
            return t.Type;
        }
    }
}
