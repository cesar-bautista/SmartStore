using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace SmartStore.Api.Infrastructure
{
    public abstract class BaseRepository<TConnection, TEntity>
        where TConnection : IDbConnection, new()
        where TEntity : class, new()
    {
        private static readonly IDictionary<Type, PropertyInfo[]> _cachePropertyList = new Dictionary<Type, PropertyInfo[]>();
        private static readonly object _syncPropertyTypeCache = new object();
        private readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            _connectionString = connectionString;
        }

        #region Crud
        public int Insert(TEntity entity)
        {
            if (entity == null)
                entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var result = InsertParameters(entity);

            if (result.Type == CommandType.Text)
                return ExecuteQuery<int>(result.Query, result.Params);
            return ExecuteStoredProcedure<int>(result.Query, result.Params);
        }

        public int Update(TEntity entity)
        {
            if (entity == null)
                entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var result = UpdateParameters(entity);

            if (result.Type == CommandType.Text)
                return ExecuteQuery<int>(result.Query, result.Params);
            return ExecuteStoredProcedure<int>(result.Query, result.Params);
        }

        public int Delete(TEntity entity)
        {
            if (entity == null)
                entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var result = DeleteParameters(entity);

            if (result.Type == CommandType.Text)
                return ExecuteQuery<int>(result.Query, result.Params);
            return ExecuteStoredProcedure<int>(result.Query, result.Params);
        }

        public TEntity GetByKey(TEntity entity)
        {
            if (entity == null)
                entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var result = SelectParameters(entity);

            if (result.Type == CommandType.Text)
                return ExecuteQuery<TEntity>(result.Query, result.Params);
            return ExecuteStoredProcedure<TEntity>(result.Query, result.Params);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var result = SelectParameters(entity);

            if (result.Type == CommandType.Text)
                return ExecuteQuery<List<TEntity>>(result.Query, result.Params);
            return ExecuteStoredProcedure<List<TEntity>>(result.Query, result.Params);
        }
        #endregion

        #region Virtual
        protected virtual CommandResult InsertParameters(TEntity entity)
        {
            var type = entity.GetType();
            var props = ObtenerPropiedadesTipo(type).Where(p => p.CanWrite).Select(s => s.Name).ToList();

            var query = $"INSERT INTO [{type.Name}]({string.Join(",", props)}) VALUES(@{string.Join(",@", props)});";

            return new CommandResult
            {
                Query = query,
                Type = CommandType.Text
            };
        }

        protected virtual CommandResult UpdateParameters(TEntity entity)
        {
            var type = entity.GetType();
            var props = ObtenerPropiedadesTipo(type).Where(p => p.CanWrite).Select(s => s.Name).ToList();

            var id = props.FirstOrDefault(s => s == "id");

            var query = $"UPDATE [{type.Name}] SET {string.Join(",", props)} WHERE {id} = -1;";

            return new CommandResult
            {
                Query = query,
                Type = CommandType.Text
            };
        }

        protected virtual CommandResult DeleteParameters(TEntity entity)
        {
            var type = entity.GetType();
            var props = ObtenerPropiedadesTipo(type).Where(p => p.CanWrite).Select(s => s.Name);

            var id = props.FirstOrDefault(s => s == "id");

            var query = $"DELETE FROM [{type.Name}] WHERE {id} = -1;";

            return new CommandResult
            {
                Query = query,
                Type = CommandType.Text
            };
        }

        protected virtual CommandResult SelectParameters(TEntity entity)
        {
            var type = entity.GetType();
            var props = ObtenerPropiedadesTipo(type).Where(p => p.CanWrite).Select(s => s.Name);
            var query = $"SELECT {string.Join(",", props)} FROM [{type.Name}];";

            return new CommandResult
            {
                Query = query,
                Type = CommandType.Text
            };
        }
        #endregion

        #region Protected
        protected void OnTransaction(Action action)
        {
            if (action == null)
                throw new InvalidOperationException($"{nameof(action)} is requiered");

            using (var tran = new TransactionScope(TransactionScopeOption.Required))
            {
                action();
                tran.Complete();
            }
        }

        ///<summary>Execute query</summary>
        ///<example>Call this method
        ///<code>
        ///ExecuteQuery("SELECT * FROM [TableName] WHERE Id=@type AND Desc=@desc", new { param1 = 1, param2 = "" });
        ///</code>
        ///</example>
        protected void ExecuteQuery(string query, object parameters = null)
        {
            var args = GetParametersAsDictionary(parameters);
            ExecuteCommand(query, CommandType.Text, args);
        }

        protected TResult ExecuteQuery<TResult>(string query, object parameters = null)
        {
            var args = GetParametersAsDictionary(parameters);
            return ExecuteCommand<TResult>(query, CommandType.Text, args);
        }

        protected void ExecuteStoredProcedure(string query, object parameters = null)
        {
            var args = GetParametersAsDictionary(parameters);
            ExecuteStoredProcedure(query, args);
        }

        protected void ExecuteStoredProcedure(string query, IDictionary<string, object> parameters)
        {
            ExecuteCommand(query, CommandType.StoredProcedure, parameters);
        }

        protected TResult ExecuteStoredProcedure<TResult>(string query, object parameters = null, bool isDeepEntity = false)
        {
            var args = GetParametersAsDictionary(parameters);
            return ExecuteStoredProcedure<TResult>(query, args, isDeepEntity);
        }

        protected TResult ExecuteStoredProcedure<TResult>(string query, IDictionary<string, object> parameters, bool isDeepEntity = false)
        {
            return ExecuteCommand<TResult>(query, CommandType.StoredProcedure, parameters, isDeepEntity);
        }
        #endregion

        #region Private
        private void ExecuteCommand(string query, CommandType commandType, IDictionary<string, object> parameters)
        {
            ExecuteInternalCommand(query, commandType, parameters, command => command.ExecuteNonQuery());
        }

        private TResult ExecuteCommand<TResult>(string query, CommandType commandType, IDictionary<string, object> parameters, bool idDeepEntity = false)
        {
            TResult results = default;

            ExecuteInternalCommand(query, commandType, parameters,
                command =>
                {
                    results = ReadCommandResults<TResult>(command, idDeepEntity);
                });

            return results;
        }

        private void ExecuteInternalCommand(string query, CommandType commandType, IDictionary<string, object> parameters, Action<IDbCommand> executeCommand)
        {
            using (var connection = new TConnection { ConnectionString = _connectionString })
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = query;

                    AddCommandParameters(command, parameters);

                    try
                    {
                        command.Connection.Open();
                        executeCommand(command);
                        UpdateParameterValues(command, parameters);
                    }
                    catch { throw; }
                    finally
                    {
                        if (command.Connection.State == ConnectionState.Open)
                        {
                            command.Connection.Close();
                            command.Dispose();
                            connection.Dispose();
                        }
                    }
                }
            }
        }

        private IDictionary<string, object> GetParametersAsDictionary(object parameters)
        {
            var result = new Dictionary<string, object>();

            if (parameters != null)
            {
                if (parameters.GetType().IsArray)
                {
                    int index = 1;
                    Array.ForEach((object[])parameters, (o) => result.Add($"@p{(index++)}", o));
                }
                else
                {
                    foreach (var pInfo in ObtenerPropiedadesTipo(parameters.GetType()))
                    {
                        var value = pInfo.GetValue(parameters, null);
                        result.Add(pInfo.Name, value);
                    }
                }
            }

            return result;
        }

        private void AddCommandParameters(IDbCommand command, IDictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameterName in parameters.Keys)
                {
                    var parameter = command.CreateParameter();
                    var procedureParameterName = parameterName;

                    if (parameterName.StartsWith("output"))
                    {
                        parameter.Direction = ParameterDirection.Output;
                        procedureParameterName = procedureParameterName.Replace("output", "");
                    }

                    //if (parameterName.StartsWith("structured", StringComparison.Ordinal))
                    //{
                    //    var sqlParameter = parameter as SqlParameter;
                    //    if (sqlParameter != null)
                    //    {
                    //        sqlParameter.SqlDbType = SqlDbType.Structured;
                    //        parameter = sqlParameter;
                    //    }
                    //    procedureParameterName = procedureParameterName.Replace("structured ", "");
                    //}

                    parameter.ParameterName = procedureParameterName;
                    parameter.Value = parameters[parameterName];

                    command.Parameters.Add(parameter);
                }
            }
        }

        private TResult ReadCommandResults<TResult>(IDbCommand command, bool isDeepEntity)
        {
            using (var reader = command.ExecuteReader())
            {
                return IsList(typeof(TResult)) ? ReadResultsIntoList<TResult>(reader) : ReadResultIntoType<TResult>(reader, isDeepEntity);
            }
        }

        private TResult ReadResultsIntoList<TResult>(IDataReader reader)
        {
            Type listElementType = GetListElementType(typeof(TResult));
            IList resultList = (IList)Activator.CreateInstance(typeof(TResult));

            while (reader.Read())
            {
                var entity = ReadResultAsType(listElementType, reader);
                resultList.Add(entity);
            }

            return (TResult)resultList;
        }

        private TResult ReadResultIntoType<TResult>(IDataReader reader, bool isDeepEntity)
        {
            if (reader.Read())
            {
                var entity = ReadResultAsType(typeof(TResult), reader, isDeepEntity);
                return (TResult)Convert.ChangeType(entity, typeof(TResult));
            }

            return default;
        }

        private object ReadResultAsType(Type entityType, IDataReader reader, bool isDeepEntity = false)
        {
            if (IsPrimitive(entityType) || IsArrayOfPrimivite(entityType))
            {
                var primitiveResult = reader.GetValue(0);
                return primitiveResult is DBNull ? null : primitiveResult;
            }
            if (entityType == typeof(ExpandoObject))
            {
                return ReadResultAsDynamic(reader);
            }
            else
            {
                return isDeepEntity ? ReadResultAsDeepEntity(reader, entityType) : ReadResultAsEntity(reader, entityType);
            }
        }

        private dynamic ReadResultAsDynamic(IDataReader reader)
        {
            dynamic dynamicEntity = new ExpandoObject();

            for (var i = 0; i < reader.FieldCount; i++)
                ((IDictionary<string, object>)dynamicEntity).Add(reader.GetName(i), reader.GetValue(i));

            return dynamicEntity;
        }

        private object ReadResultAsDeepEntity(IDataReader reader, Type entityType)
        {
            var properties = GetNestedPropertiesType(entityType);

            var foundObjects = new Dictionary<Type, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);

                PropertyInfo pInfo = null;

                var fullType = properties.FirstOrDefault(p =>
                {
                    pInfo = p.First(name);
                    return pInfo != null;
                });

                if (fullType != null)
                {
                    object temp = null;

                    if (!foundObjects.ContainsKey(fullType.EntiryType))
                        temp = Activator.CreateInstance(fullType.EntiryType);
                    else
                        temp = foundObjects[fullType.EntiryType];

                    if (pInfo != null)
                    {
                        var fieldValue = reader.GetValue(i);
                        var dotNetTypeValue = fieldValue is DBNull ? null : ConvertToValueType(fieldValue, pInfo.PropertyType);
                        pInfo.SetValue(temp, dotNetTypeValue, null);
                    }

                    if (!foundObjects.ContainsKey(fullType.EntiryType))
                        foundObjects.Add(fullType.EntiryType, temp);
                }
            }

            return MapNestedObjects(entityType, foundObjects);
        }

        private object ReadResultAsEntity(IDataReader reader, Type entityType)
        {
            var entity = Activator.CreateInstance(entityType);
            var properties = ObtenerPropiedadesTipo(entityType).Where(p => p.CanWrite);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var pInfo = properties.FirstOrDefault(p => string.Compare(p.Name, name, true) == 0);

                if (pInfo != null)
                {
                    var fieldValue = reader.GetValue(i);
                    var typeValueNet = fieldValue is DBNull ? null : ConvertToValueType(fieldValue, pInfo.PropertyType);
                    pInfo.SetValue(entity, typeValueNet, null);
                }
            }

            return entity;
        }

        private void UpdateParameterValues(IDbCommand command, IDictionary<string, object> parameters)
        {
            if (command.CommandType == CommandType.StoredProcedure)
            {
                foreach (IDataParameter parameter in command.Parameters)
                {
                    if (parameters.ContainsKey(parameter.ParameterName) || parameters.ContainsKey($"output{parameter.ParameterName}"))
                    {
                        parameters[parameter.ParameterName] = parameter.Value;
                    }
                }
            }
        }

        #region Type
        private static Type GetListElementType(Type type)
        {
            if (typeof(IList).IsAssignableFrom(type) && type.IsGenericType)
            {
                return type.GetGenericArguments()[0];
            }

            throw new ArgumentException("Result type is not valid");
        }

        private object ConvertToValueType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Convert.ChangeType(value, type.GetGenericArguments()[0]);

            return Convert.ChangeType(value, type);
        }

        private static PropertyInfo[] ObtenerPropiedadesTipo(Type type)
        {
            if (!_cachePropertyList.TryGetValue(type, out PropertyInfo[] propertiesInType))
            {
                lock (_syncPropertyTypeCache)
                {
                    if (!_cachePropertyList.TryGetValue(type, out propertiesInType))
                    {
                        propertiesInType = type.GetProperties();
                        _cachePropertyList.Add(type, propertiesInType);
                    }
                }
            }
            return propertiesInType;
        }

        private static List<PropertyType> GetNestedPropertiesType(Type type)
        {
            var resultsList = new List<PropertyType>();
            var propertiesList = new List<PropertyInfo>();

            foreach (var prop in type.GetProperties())
            {
                if (!IsPrimitive(prop.PropertyType))
                {
                    resultsList.AddRange(GetNestedPropertiesType(prop.PropertyType));
                }
                else
                {
                    propertiesList.Add(prop);
                }
            }

            resultsList.Add(new PropertyType
            {
                EntiryType = type,
                Properties = propertiesList.ToArray()
            });

            return resultsList;
        }

        private static object MapNestedObjects(Type type, Dictionary<Type, object> findObjects)
        {
            if (findObjects.ContainsKey(type))
            {
                var currentInstance = findObjects[type];

                foreach (var prop in type.GetProperties())
                {
                    if (!IsPrimitive(prop.PropertyType))
                    {
                        var value = MapNestedObjects(prop.PropertyType, findObjects);
                        prop.SetValue(currentInstance, value);
                    }
                }

                return currentInstance;
            }

            return null;
        }

        private bool IsList(Type type)
        {
            return !IsPrimitive(type) && typeof(IEnumerable).IsAssignableFrom(type) && !type.IsArray;
        }

        private static bool IsPrimitive(Type entityType)
        {
            return entityType.IsPrimitive || entityType.IsEnum || IsFrameworkType(entityType);
        }

        private static bool IsArrayOfPrimivite(Type entityType)
        {
            if (entityType.IsArray)
            {
                return IsPrimitive(entityType.GetElementType());
            }
            return false;
        }

        private static bool IsFrameworkType(Type entityType)
        {
            return Type.GetTypeCode(entityType) == TypeCode.String || Type.GetTypeCode(entityType) == TypeCode.DateTime || Type.GetTypeCode(entityType) == TypeCode.Decimal;
        }
        #endregion
        #endregion

        internal class PropertyType
        {
            public Type EntiryType { get; set; }
            public PropertyInfo[] Properties { get; set; }

            public PropertyInfo First(string propertyName)
            {
                if (Properties != null)
                    return Properties.FirstOrDefault(p => string.Compare(p.Name, propertyName, true) == 0);

                return null;
            }
        }

        protected class CommandResult
        {
            public string Query { get; set; }
            public object Params { get; set; }
            public CommandType Type { get; set; }
        }
    }
}