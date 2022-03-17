/*
 * Copyright(c) 2017 Microsoft Corporation. All rights reserved. 
 * 
 * This code is licensed under the MIT License (MIT). 
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do 
 * so, subject to the following conditions: 
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE. 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace RESTToolkitTestApp
{
    /// <summary>
    /// Takes any Object and generates an Object Tree.
    /// </summary>
    public class ObjectNode
    {
        #region Private Properties

        private string _name;
        private object _value;
        private Type _type;

        #endregion

        #region Constructor

        /// <summary>
        /// Takes any Object and generates an Object Tree.
        /// </summary>
        /// <param name="value">The object to parse into an Object Tree.</param>
        public ObjectNode(object value)
        {
            ParseObjectTree("root", value, value.GetType());
        }

        /// <summary>
        /// Takes any Object and generates an Object Tree.
        /// </summary>
        /// <param name="name">The name of the root node.</param>
        /// <param name="value">The object to parse into an Object Tree.</param>
        public ObjectNode(string name, object value)
        {
            ParseObjectTree(name, value, value.GetType());
        }

        /// <summary>
        /// Takes any Object and generates an Object Tree.
        /// </summary>
        /// <param name="value">The object to parse into an Object Tree.</param>
        /// <param name="t">The known type of the object.</param>
        public ObjectNode(object value, Type t)
        {
            ParseObjectTree("root", value, t);
        }

        /// <summary>
        /// Takes any Object and generates an Object Tree.
        /// </summary>
        /// <param name="name">The name of the root node.</param>
        /// <param name="value">The object to parse into an Object Tree.</param>
        /// <param name="t">The known type of the object.</param>
        public ObjectNode(string name, object value, Type t)
        {
            ParseObjectTree(name, value, t);
        }

        #endregion

        /// <summary>
        /// Takes any Object and generates an Object Tree.
        /// </summary>
        /// <param name="name">The name of the root node.</param>
        /// <param name="value">The object to parse into an Object Tree.</param>
        public static async Task<ObjectNode> ParseAsync(string name, object value)
        {
            return await Task.Run<ObjectNode>(() =>
            {
                return new ObjectNode(name, value);
            });
        }

        #region Public Properties

        /// <summary>
        /// The property name of the object.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// The value of the object.
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// The type of the object.
        /// </summary>
        public Type Type
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// A list of child objects in the object.
        /// </summary>
        public List<ObjectNode> Children { get; set; }

        #endregion

        #region Private Methods

        private void ParseObjectTree(string name, object value, Type type)
        {
            Children = new List<ObjectNode>();

            _type = type;
            _name = name;

            if (value != null)
            {
                if (value is string && type != typeof(object))
                {
                    if (value != null)
                    {
                        _value = "\"" + value + "\"";
                    }
                }
                else if (value is double || value is bool || value is int || value is float || value is long || value is decimal)
                {
                    _value = value;
                }
                else if (value is DateTime)
                {
                    try
                    {
                        _value = ((DateTime)value).ToString("o");
                    }
                    catch { }
                }
                else
                {
                    _value = "{" + value.ToString() + "}";
                }
            }

            PropertyInfo[] props = type.GetProperties();

            if (props.Length == 0 && type.IsClass && value is IEnumerable && !(value is string))
            {
                IEnumerable arr = value as IEnumerable;

                if (arr != null)
                {
                    int i = 0;
                    foreach (object element in arr)
                    {
                        Children.Add(new ObjectNode("[" + i + "]", element, element.GetType()));
                        i++;
                    }

                }
            }

            foreach (PropertyInfo p in props)
            {
                if (p.PropertyType.IsPublic)
                {
                    if (p.PropertyType.IsClass || p.PropertyType.IsArray || p.PropertyType.FullName.StartsWith("System.Collections.Generic.List"))
                    {
                        if (p.PropertyType.IsArray || p.PropertyType.FullName.StartsWith("System.Collections.Generic.List"))
                        {
                            try
                            {
                                object v = p.GetValue(value, null);
                                IEnumerable arr = v as IEnumerable;

                                if (arr != null)
                                {
                                    ObjectNode arrayNode = new ObjectNode(p.Name, arr.ToString(), typeof(object));

                                    int i = 0, k = 0;
                                    ObjectNode arrayNode2;

                                    foreach (object element in arr)
                                    {
                                        //Handle 2D arrays
                                        if (element is IEnumerable && !(element is string))
                                        {
                                            arrayNode2 = new ObjectNode("[" + i + "]", element.ToString(), typeof(object));

                                            IEnumerable arr2 = element as IEnumerable;
                                            k = 0;

                                            foreach (object e in arr2)
                                            {
                                                arrayNode2.Children.Add(new ObjectNode("[" + k + "]", e, e.GetType()));
                                                k++;
                                            }

                                            arrayNode.Children.Add(arrayNode2);
                                        }
                                        else
                                        {
                                            arrayNode.Children.Add(new ObjectNode("[" + i + "]", element, element.GetType()));
                                        }
                                        i++;
                                    }

                                    Children.Add(arrayNode);
                                }
                            }
                            catch (Exception e){
                                var t = e;
                            }
                        }
                        else
                        {
                            object v = p.GetValue(value, null);

                            if (v != null)
                            {
                                Children.Add(new ObjectNode(p.Name, v, p.PropertyType));
                            }
                        }
                    }
                    else if (p.PropertyType.IsValueType && !(value is string) && !(value is DateTime))
                    {
                        try
                        {
                            object v = p.GetValue(value, null);

                            if (v != null)
                            {
                                Children.Add(new ObjectNode(p.Name, v, p.PropertyType));
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        #endregion
    }
}
