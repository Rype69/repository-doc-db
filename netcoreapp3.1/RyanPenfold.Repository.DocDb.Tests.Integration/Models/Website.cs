// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Website.cs" company="Ryan Penfold">
//   Copyright © Ryan Penfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Models
{
    /// <summary>
    /// Mapped from a database object.
    /// </summary>
    public class Website
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public virtual System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public virtual string Name { get; set; }
    }
}