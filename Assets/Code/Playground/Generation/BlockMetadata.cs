using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Playground.Generation
{
    public class ParameterMetadata
    {
        public readonly int Position;
        public readonly string Name;
        public readonly string Description;
        public readonly Type Type;
        public readonly ParameterType ParamType;
        public readonly ParameterInfo parameterInfo;

        public enum ParameterType
        {
            Input,
            Output,
            InReadOnly
        }

        public ParameterMetadata(ParameterInfo pi)
        {
            parameterInfo = pi;
            Position = pi.Position;

            var nameattrib = pi.GetCustomAttribute<NameAttribute>();
            if (nameattrib != null) Name = nameattrib.Value;
            else Name = pi.Name;

            var descattrib = pi.GetCustomAttribute<DescriptionAttribute>();
            if (descattrib != null) Description = descattrib.Value;
            else Description = String.Empty;

            Type = pi.ParameterType;

            if (pi.IsIn)
                ParamType = ParameterType.InReadOnly;
            else if (pi.IsOut)
                ParamType = ParameterType.Output;
            else
                ParamType = ParameterType.Input;
        }
    }

    public class MethodMetadata
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string FullString;
        public readonly ParameterMetadata[] Inputs;
        public readonly ParameterMetadata[] Outputs;
        public readonly int ParameterCount;
        public readonly MethodInfo methodInfo;

        // TODO definitely slow
        public void Execute(object instance, object[] args)
        {
            methodInfo.Invoke(instance, args);
        }

        public MethodMetadata(MethodInfo mi)
        {
            methodInfo = mi;

            var nameattrib = mi.GetCustomAttribute<NameAttribute>();
            if (nameattrib != null) Name = nameattrib.Value;
            else Name = mi.Name;

            var descattrib = mi.GetCustomAttribute<DescriptionAttribute>();
            if (descattrib != null) Description = descattrib.Value;
            else Description = String.Empty;

            var ins = new List<ParameterMetadata>();
            var outs = new List<ParameterMetadata>();
            foreach (var p in mi.GetParameters())
            {
                var n = new ParameterMetadata(p);
                if (n.ParamType == ParameterMetadata.ParameterType.Output)
                    outs.Add(n);
                else
                    ins.Add(n);
            }

            Inputs = ins.ToArray();
            Outputs = outs.ToArray();

            var sb = new StringBuilder();
            sb.Append($"{Name} in:(");
            for (int i = 0; i < Inputs.Length; i++)
            {
                sb.Append($"{Inputs[i].Type.Name} {Inputs[i].Name}");
                if (i == Inputs.Length - 1)
                    sb.Append(",");
            }
            sb.Append(") out:(");
            for (int i = 0; i < Outputs.Length; i++)
            {
                sb.Append($"{Outputs[i].Type.Name} {Outputs[i].Name}");
                if (i == Inputs.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");

            FullString = sb.ToString();
        }
    }

    public class FieldMetadata
    {
        public readonly string Name;
        public readonly string Description;
        public readonly Type Type;
        public readonly FieldInfo fieldInfo;

        // TODO is this slow? maybe...
        public object GetValue(object instance)
        {
            return fieldInfo.GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            fieldInfo.SetValue(instance, value);
        }

        public FieldMetadata(FieldInfo fi)
        {
            var nameattrib = fi.GetCustomAttribute<NameAttribute>();
            if (nameattrib != null) Name = nameattrib.Value;
            else Name = fi.Name;

            var descattrib = fi.GetCustomAttribute<DescriptionAttribute>();
            if (descattrib != null) Description = descattrib.Value;
            else Description = String.Empty;

            fieldInfo = fi;
            Type = fi.FieldType;
        }
    }

    public class BlockMetadata
    {
        public readonly Type BlockType;
        public readonly string Name;
        public readonly string Description;

        public readonly FieldMetadata[] Fields;
        public readonly MethodMetadata[] Methods;

        public BlockMetadata(Type type)
        {
            if (!type.IsSubclassOf(typeof(Block)))
                throw new ArgumentException();

            var nameattrib = type.GetCustomAttribute<NameAttribute>();
            if (nameattrib != null) Name = nameattrib.Value;
            else Name = type.Name;

            var descattrib = type.GetCustomAttribute<DescriptionAttribute>();
            if (descattrib != null) Description = descattrib.Value;
            else Description = String.Empty;

            var fields = new List<FieldMetadata>();
            foreach (FieldInfo fi in type.GetRuntimeFields())
            {
                if (fi.IsStatic || fi.GetCustomAttribute<BlockVariableAttribute>() == null)
                    continue;

                fields.Add(new FieldMetadata(fi));
            }
            Fields = fields.ToArray();

            var methods = new List<MethodMetadata>();
            foreach (MethodInfo mi in type.GetRuntimeMethods())
            {
                if (mi.IsStatic || mi.GetCustomAttribute<BlockMethodAttribute>() == null)
                    continue;

                methods.Add(new MethodMetadata(mi));
            }
            Methods = methods.ToArray();

            BlockType = type;
        }
    }
}
