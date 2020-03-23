// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Article.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Models
{
    /// <summary>
    /// Mapped from a database object.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public virtual System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the parent <see cref="Article"/>.
        /// </summary>
        public virtual Article Parent { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a parent <see cref="Article"/>.
        /// </summary>
        public virtual System.Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets an identifier, if any, of a specific thread (conversation etc)
        /// that this <see cref="Article"/> pertains to.
        /// </summary>
        public virtual System.Guid? ThreadId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this <see cref="Article"/> was posted.
        /// </summary>
        public virtual System.DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier, if any, of a user that posted this article.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the rating of the <see cref="Article"/>.
        /// </summary>
        public virtual byte Rating { get; set; }

        /// <summary>
        /// Gets or sets the name of a type this 
        /// <see cref="Article"/> relates to, if any.
        /// </summary>
        public virtual string RelatesToType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a type instance this 
        /// <see cref="Article"/> relates to, if any.
        /// </summary>
        public virtual string RelatesToId { get; set; }

        /// <summary>
        /// Gets or sets the approval status of the <see cref="Article"/>.
        /// </summary>
        public virtual int ApprovalStatus { get; set; }

        /// <summary>
        /// Gets or sets the identifier of user, if specified,
        /// that approved this <see cref="Article"/>.
        /// </summary>
        public virtual string ApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time, if any, that this 
        /// <see cref="Article"/> approved.
        /// </summary>
        public virtual System.DateTime? DateApproved { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the website to which this <see cref="Article"/> pertains.
        /// </summary>
        public virtual System.Guid WebsiteId { get; set; }
    }
}