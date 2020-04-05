// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRepository{T}.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RyanPenfold.BusinessBase.Infrastructure;

namespace RyanPenfold.Repository.DocDb
{
    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="T">An entity type</typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The data set
        /// </summary>
        protected List<T> Data;

        /// <summary>
        /// The path to the data file.
        /// </summary>
        protected string FilePath;

        /// <summary>
        /// A file service instance.
        /// </summary>
        protected FileService FileService;

        /// <summary>
        /// A serialiser service instance.
        /// </summary>
        protected SerialiserService SerialiserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class. 
        /// </summary>
        /// <param name="typeMap">
        /// Mapping information for type T
        /// </param>
        protected BaseRepository(BaseClassMap<T> typeMap)
        {
            TypeMap = typeMap ?? throw new ArgumentNullException(nameof(typeMap));
        }

        /// <inheritdoc />
        public IClassMap TypeMap { get; set; }

        /// <inheritdoc />
        public long Count()
        {
            // Load the data
            Load();

            return Data?.Count ?? 0;
        }

        /// <inheritdoc />
        public IList<T> FindAll()
        {
            // Load the data
            Load();

            // Return all data
            return Data;
        }

        /// <inheritdoc />
        public T FindById(object id)
        {
            // Load the data
            Load();

            // Result object
            T rtn = null;

            // Determine id property
            foreach (var propertyName in TypeMap.PrimaryKeyColumnNames)
            {
                // Attempt to get the id property info for the type
                var propertyInfo = typeof(T).GetProperty(propertyName);

                // NULL-check the property info
                if (propertyInfo == null)
                {
                    throw new MissingMemberException(typeof(T).Name, propertyName);
                }

                // Find
                rtn = Data.FirstOrDefault(e => propertyInfo.GetValue(e) != null && propertyInfo.GetValue(e).ToString() == id.ToString());

                if (rtn != null)
                    break;
            }

            // Return result
            return rtn;
        }

        /// <inheritdoc />
        public IList<T> FindByIds(object[] ids)
        {
            // Load the data
            Load();

            // Result object
            var rtn = new List<T>();

            // Loop through each identifier
            foreach (var id in ids)
            {
                // Determine id property
                foreach (var propertyName in TypeMap.PrimaryKeyColumnNames)
                {
                    // Attempt to get the id property info for the type
                    var propertyInfo = typeof(T).GetProperty(propertyName);

                    // NULL-check the property info
                    if (propertyInfo == null)
                    {
                        throw new MissingMemberException(typeof(T).Name, propertyName);
                    }

                    // Find
                    var entity = Data.FirstOrDefault(e => propertyInfo.GetValue(e) != null && propertyInfo.GetValue(e).ToString() == id.ToString());

                    // Add entity to results if it is found
                    if (entity != null)
                        rtn.Add(entity);
                }
            }

            // Return result
            return rtn;
        }

        /// <inheritdoc />
        public TValue FindMaxValue<TValue>(string columnName)
        {
            // Load the data
            Load();

            // Attempt to get the id property info for the type
            var propertyInfo = typeof(T).GetProperty(columnName);

            // If property not found, return default
            if (propertyInfo == null)
                return default(TValue);

            // Determine maximum value
            TValue Func(T p) => (TValue)p.GetType().GetProperty(columnName)?.GetValue(p);

            // Return result
            return Data.Max(Func);
        }

        /// <inheritdoc />
        public string NewId(string propertyName)
        {
            // Result variable
            string newIdentifier;

            // Attempt to get the property info for the type
            var propertyInfo = typeof(T).GetProperty(propertyName);

            // NULL-check the property info
            if (propertyInfo == null)
            {
                throw new MissingMemberException(typeof(T).Name, propertyName);
            }

            // Get all the entities of this type from the data store
            FindAll();

            // Generate a unique identifier for the new entity
            do
            {
                newIdentifier = Guid.NewGuid().ToString();
            }
            while (Data != null && Data.Any(e => string.Equals(propertyInfo.GetValue(e).ToString(), newIdentifier, StringComparison.InvariantCultureIgnoreCase)));

            // Return result
            return newIdentifier;
        }

        /// <inheritdoc />
        public void Remove(T entity)
        {
            Load();

            T retrievedEntity = null;

            // Attempt to get the Id property
            foreach (var idPropertyName in TypeMap.PrimaryKeyColumnNames)
            {
                var idPropertyInfo = typeof(T).GetProperty(idPropertyName);
                if (idPropertyInfo != null)
                {
                    var idPropertyValue = idPropertyInfo.GetValue(entity);
                    if (idPropertyValue == null || idPropertyValue.ToString() == Guid.Empty.ToString())
                        idPropertyInfo.SetValue(entity, NewId(idPropertyName));

                    if (Data == null)
                        return;

                    retrievedEntity = Data.FirstOrDefault(e => string.Equals(idPropertyInfo.GetValue(e).ToString(),
                        idPropertyInfo.GetValue(entity).ToString(), StringComparison.InvariantCultureIgnoreCase));

                    if (retrievedEntity != null)
                        Data.Remove(retrievedEntity);

                    break;
                }
            }

            if (retrievedEntity == null)
                throw new EntityNotFoundException("The specified entity is not found in the repository");

            Save();
        }

        /// <inheritdoc />
        public void RemoveBy(long id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Save(T entity)
        {
            try
            {
                Remove(entity);
            }
            catch (EntityNotFoundException)
            {
            }

            Data.Add(entity);

            Save();
        }

        #region Protected methods

        /// <summary>
        /// Ensures there are no nested instances of <see cref="T"/> in the <see cref="Data"/>.
        /// </summary>
        protected void Flatten()
        {
            if (Data == null || !Data.Any(d => d != null))
                return;

            foreach (var datum in Data)
            {
                if (datum == null)
                    continue;

                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.PropertyType == typeof(T))
                        property.SetValue(datum, null);
                }
            }
        }

        /// <summary>
        /// Initialises file writing objects
        /// </summary>
        protected void Init()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
            {
                var fileName = (TypeMap as BaseClassMap<T>)?.DeriveFileName();

                var directoryPath = Configuration.ConfigurationSettings.DataDirectoryPath;
                if (string.IsNullOrWhiteSpace(directoryPath) || string.Equals(directoryPath, "\\"))
                    directoryPath = Utilities.IO.UncPath.GetApplicationDirectory();
                else if (!string.IsNullOrWhiteSpace(directoryPath) && directoryPath.StartsWith("\\"))
                    directoryPath = $"{Utilities.IO.UncPath.GetApplicationDirectory()}{directoryPath}";
                else if (!string.IsNullOrWhiteSpace(directoryPath) && directoryPath.StartsWith(".."))
                    directoryPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), directoryPath));

                if (fileName != null)
                    FilePath = System.IO.Path.Combine(directoryPath, fileName);
            }

            if (FileService == null)
                FileService = new FileService();

            if (SerialiserService == null)
                SerialiserService = new SerialiserService();
        }

        /// <summary>
        /// Links all nested instances of <see cref="T"/> in the <see cref="Data"/>.
        /// </summary>
        protected void Link()
        {
            if (Data == null || Data.All(d => d == null))
                return;

            foreach (var datum in Data)
            {
                if (datum == null)
                    continue;

                Link(datum);
            }
        }

        /// <summary>
        /// Links all nested instances of <see cref="T"/> in the <see cref="Data"/>.
        /// </summary>
        protected void Link(T datum)
        {
            var allPropertiesOfType = typeof(T).GetProperties();
            foreach (var property in allPropertiesOfType)
            {
                if (property.PropertyType == typeof(T) && property.GetValue(datum) == null)
                {
                    // TODO: Specific relationships could be configured in the ClassMap
                    var foreignKeyProperty =
                        allPropertiesOfType.FirstOrDefault(p => p.Name == $"{property.Name}Id");
                    if (foreignKeyProperty != null && foreignKeyProperty.GetValue(datum) != null)
                    {
                        var foreignId = foreignKeyProperty.GetValue(datum);
                        var entity = FindById(foreignId);
                        if (entity != null)
                        {
                            property.SetValue(datum, entity);
                            Link(entity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attempts ot load all data from the store
        /// </summary>
        protected void Load()
        {
            Init();
            Link();

            if (System.IO.File.Exists(FilePath))
            {
                var serialisedData = SerialiserService.GetString(FileService.Read(FilePath).Result);
                Data = JsonConvert.DeserializeObject<List<T>>(serialisedData, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            else
            {
                Data = new List<T>();
            }
        }

        /// <summary>
        /// Attempts ot save all data to the store
        /// </summary>
        protected void Save()
        {
            Init();
            Flatten();

            var serialisedData = JsonConvert.SerializeObject(Data, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            FileService.Write(FilePath, SerialiserService.GetBytes(serialisedData));
        }

        #endregion Protected methods
    }
}
