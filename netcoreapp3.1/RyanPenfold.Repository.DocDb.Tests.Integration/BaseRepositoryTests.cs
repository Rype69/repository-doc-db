// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRepositoryTests.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RyanPenfold.BusinessBase.Infrastructure;
using RyanPenfold.Repository.DocDb.Tests.Integration.Models;
using RyanPenfold.Repository.DocDb.Tests.Integration.Repository;
using RyanPenfold.Repository.DocDb.Tests.Integration.Repository.Mappings;

namespace RyanPenfold.Repository.DocDb.Tests.Integration
{
    /// <summary>
    /// Provides tests for the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    [TestClass]
    public class BaseRepositoryTests
    {
        /// <summary>
        /// Tests the constructor of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        [TestMethod]
        public void Ctor_NoParameters_TypeMapPropertySet()
        {
            // Act
            var articleRepository = new ArticleRepository();

            // Assert
            Assert.IsTrue(articleRepository.TypeMap is ArticleMap);
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.Count"/> method.
        /// </summary>
        [TestMethod]
        public void Count_DataAbsent_ZeroReturned()
        {
            // Arrange
            var filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
            Utilities.IO.File.Delete(filePath);
            var articleRepository = new ArticleRepository();

            // Act
            var count = articleRepository.Count();

            // Assert
            Assert.AreEqual(0, count);
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.Count"/> method.
        /// </summary>
        [TestMethod]
        public void Count_DataPresent_CorrectNumberReturned()
        {
            // Arrange
            var expectedCount = 2;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < expectedCount; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();

                // Act
                var count = articleRepository.Count();

                // Assert
                Assert.AreEqual(expectedCount, count);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindAll"/> method.
        /// </summary>
        [TestMethod]
        public void FindAll_DataAbsent_EmptyListReturned()
        {
            // Arrange
            var filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
            Utilities.IO.File.Delete(filePath);
            var articleRepository = new ArticleRepository();

            // Act
            var results = articleRepository.FindAll();

            // Assert
            Assert.IsFalse(results == null);
            Assert.IsTrue(results is List<Article>);
            Assert.IsFalse(results.Any(a => a != null));
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindAll"/> method.
        /// </summary>
        [TestMethod]
        public void FindAll_DataPresent_AllDataReturned()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();

                // Act
                var results = articleRepository.FindAll();

                // Assert
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                foreach (var result in results)
                {
                    var sourceObject = articles.FirstOrDefault(a => a.Id == result.Id);
                    Assert.IsTrue(comparer.Equals(sourceObject, result));
                }
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindAll"/> method.
        /// </summary>
        [TestMethod]
        public void FindById_MatchingRecordAbsent_ReturnsNull()
        {
            // Arrange
            var filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
            Utilities.IO.File.Delete(filePath);
            var articleRepository = new ArticleRepository();

            // Act
            var result = articleRepository.FindById(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindAll"/> method.
        /// </summary>
        [TestMethod]
        public void FindById_MatchingRecordPresent_ReturnsInstance()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var sourceObject = articles.First(a => a != null);

                // Act
                var result = articleRepository.FindById(sourceObject.Id);

                // Assert
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                Assert.IsTrue(comparer.Equals(sourceObject, result));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindById"/> method.
        /// </summary>
        [TestMethod]
        public void FindById_DataHasParentItem_ParentItemPresentInObject()
        {
            // Arrange
            string filePath = null;
            try
            {
                var parentArticle = new Article
                {
                    ApprovalStatus = 444,
                    ApprovedBy = "Ryan",
                    DateApproved = DateTime.Now,
                    DateTime = DateTime.Now.AddDays(-1),
                    Id = Guid.NewGuid(),
                    Rating = 5,
                    RelatesToId = Guid.NewGuid().ToString(),
                    RelatesToType = "Test",
                    ThreadId = Guid.NewGuid(),
                    UserId = Guid.NewGuid().ToString(),
                    WebsiteId = Guid.NewGuid()
                };

                var article = new Article
                {
                    ApprovalStatus = 444,
                    ApprovedBy = "Ryan",
                    DateApproved = DateTime.Now,
                    DateTime = DateTime.Now.AddDays(-1),
                    Id = Guid.NewGuid(),
                    Parent = parentArticle,
                    ParentId = parentArticle.Id,
                    Rating = 5,
                    RelatesToId = Guid.NewGuid().ToString(),
                    RelatesToType = "Test",
                    ThreadId = Guid.NewGuid(),
                    UserId = Guid.NewGuid().ToString(),
                    WebsiteId = Guid.NewGuid()
                };

                var articles = new List<Article>();
                articles.Add(parentArticle);
                articles.Add(article);
                var serialisedArticles = JsonConvert.SerializeObject(articles, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();

                // Act
                var result = articleRepository.FindById(article.Id);

                // Assert
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                Assert.IsTrue(comparer.Equals(article, result));
                Assert.IsTrue(comparer.Equals(article.Parent, parentArticle));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindById"/> method.
        /// </summary>
        [TestMethod]
        public void FindById_AbsoluteDirectoryPathExplicitlySpecified_RepositoryLooksAtCorrectDirectory()
        {
            // Arrange
            var originalDirectoryPath = Configuration.ConfigurationSettings.DataDirectoryPath;
            var originalFilePath = System.IO.Path.Combine(originalDirectoryPath, "[Foundation].[Article].json");
            string filePath = null;
            var count = 10;

            try
            {
                if (System.IO.File.Exists(originalFilePath))
                    Utilities.IO.File.Delete(originalFilePath);

                var newDirectoryPath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "blah1\\blah2");
                Utilities.IO.Directory.Create(newDirectoryPath);
                Configuration.ConfigurationSettings.DataDirectoryPath = newDirectoryPath;

                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(newDirectoryPath, "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var sourceObject = articles.First(a => a != null);

                // Act
                var result = articleRepository.FindById(sourceObject.Id);

                // Assert
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                Assert.IsTrue(comparer.Equals(sourceObject, result));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);

                if (!string.IsNullOrWhiteSpace(Configuration.ConfigurationSettings.DataDirectoryPath))
                    Utilities.IO.Directory.Delete(System.IO.Path.GetDirectoryName(Configuration.ConfigurationSettings.DataDirectoryPath));

                Configuration.ConfigurationSettings.DataDirectoryPath = originalDirectoryPath;
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindById"/> method.
        /// </summary>
        [TestMethod]
        public void FindById_RelativeDirectoryPathExplicitlySpecified_RepositoryLooksAtCorrectDirectory()
        {
            // Arrange
            var originalDirectoryPath = Configuration.ConfigurationSettings.DataDirectoryPath;
            var originalFilePath = System.IO.Path.Combine(originalDirectoryPath, "[Foundation].[Article].json");
            string newAbsoluteDirectoryPath = null;
            string filePath = null;
            var count = 10;

            try
            {
                if (System.IO.File.Exists(originalFilePath))
                    Utilities.IO.File.Delete(originalFilePath);

                var newRelativeDirectoryPath = "\\blah444\\blah555";
                newAbsoluteDirectoryPath = $"{Utilities.IO.UncPath.GetApplicationDirectory()}{newRelativeDirectoryPath}";
                Utilities.IO.Directory.Create(newAbsoluteDirectoryPath);
                Configuration.ConfigurationSettings.DataDirectoryPath = newRelativeDirectoryPath;

                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(newAbsoluteDirectoryPath, "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var sourceObject = articles.First(a => a != null);

                // Act
                var result = articleRepository.FindById(sourceObject.Id);

                // Assert
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                Assert.IsTrue(comparer.Equals(sourceObject, result));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);

                if (!string.IsNullOrWhiteSpace(newAbsoluteDirectoryPath))
                    Utilities.IO.Directory.Delete(System.IO.Path.GetDirectoryName(newAbsoluteDirectoryPath));

                Configuration.ConfigurationSettings.DataDirectoryPath = originalDirectoryPath;
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindById"/> method.
        /// </summary>
        [TestMethod]
        public void FindById_RelativeUpperDirectoryPathExplicitlySpecified_RepositoryLooksAtCorrectDirectory()
        {
            // Arrange
            var originalDirectoryPath = Configuration.ConfigurationSettings.DataDirectoryPath;
            var originalFilePath = System.IO.Path.Combine(originalDirectoryPath, "[Foundation].[Article].json");
            string newAbsoluteDirectoryPath = null;
            string filePath = null;
            var count = 10;

            try
            {
                if (System.IO.File.Exists(originalFilePath))
                    Utilities.IO.File.Delete(originalFilePath);

                var newRelativeDirectoryPath = "..\\data444";
                var combinedPath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), newRelativeDirectoryPath);
                newAbsoluteDirectoryPath = System.IO.Path.GetFullPath(combinedPath);
                if (System.IO.Directory.Exists(newAbsoluteDirectoryPath))
                    Utilities.IO.Directory.Delete(newAbsoluteDirectoryPath);
                Utilities.IO.Directory.Create(newAbsoluteDirectoryPath);
                Configuration.ConfigurationSettings.DataDirectoryPath = newRelativeDirectoryPath;

                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(newAbsoluteDirectoryPath, "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var sourceObject = articles.First(a => a != null);

                // Act
                var result = articleRepository.FindById(sourceObject.Id);

                // Assert
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                Assert.IsTrue(comparer.Equals(sourceObject, result));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);

                if (!string.IsNullOrWhiteSpace(newAbsoluteDirectoryPath))
                    Utilities.IO.Directory.Delete(newAbsoluteDirectoryPath);

                Configuration.ConfigurationSettings.DataDirectoryPath = originalDirectoryPath;
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindByIds"/> method.
        /// </summary>
        [TestMethod]
        public void FindByIds_MatchingRecordsPresent_ReturnsInstances()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var sourceObject1 = articles[0];
                var sourceObject2 = articles[1];

                // Act
                var results = articleRepository.FindByIds(new object[] {sourceObject1.Id, sourceObject2.Id});

                // Assert
                var result1 = results.FirstOrDefault(r => r.Id == sourceObject1.Id);
                var result2 = results.FirstOrDefault(r => r.Id == sourceObject2.Id);
                var comparer = new Utilities.Collections.EqualityComparer<Article>();
                Assert.AreEqual(2, results.Count);
                Assert.IsTrue(comparer.Equals(sourceObject1, result1));
                Assert.IsTrue(comparer.Equals(sourceObject2, result2));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.FindMaxValue{TValue}"/> method.
        /// </summary>
        [TestMethod]
        public void FindMaxValue_DataPresent_ReturnsEntityWithMaximumValue()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var approvalStatus = 0;
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = approvalStatus,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });

                    approvalStatus += 100;
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var sourceObject = articles.OrderByDescending(a => a.ApprovalStatus).First();

                // Act
                var result = articleRepository.FindMaxValue<int>("ApprovalStatus");

                // Assert
                Assert.AreEqual(sourceObject.ApprovalStatus, result);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.NewId"/> method.
        /// </summary>
        [TestMethod]
        public void NewId_DataAbsent_ArbitraryIdGenerated()
        {
            // Arrange
            var articleRepository = new ArticleRepository();

            // Act
            var result = articleRepository.NewId("Id");

            // Assert
            Guid output;
            Assert.IsTrue(Guid.TryParse(result, out output));
            Assert.IsTrue(output != Guid.Empty);
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.NewId"/> method.
        /// </summary>
        [TestMethod]
        public void NewId_DataPresent_UniqueIdGenerated()
        {
            // Arrange
            var expectedCount = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < expectedCount; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();

                // Act
                var result = articleRepository.NewId("Id");

                // Assert
                Assert.IsFalse(articles.Any(a => a.Id.ToString() == result));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.Remove"/> method.
        /// </summary>
        [TestMethod]
        public void Remove_EntityPresent_EntityRemoved()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var article = articles.First();
                var resultsBeforeRemove = articleRepository.FindAll();
                Assert.AreEqual(1, resultsBeforeRemove.Count(a => a.Id == article.Id));

                // Act
                articleRepository.Remove(article);

                // Assert
                var resultsAfterRemove = articleRepository.FindAll();
                Assert.AreEqual(0, resultsAfterRemove.Count(a => a.Id == article.Id));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.Remove"/> method.
        /// </summary>
        [ExpectedException(typeof(EntityNotFoundException))]
        [TestMethod]
        public void Remove_EntityAbsent_ExceptionThrown()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();
                var article = new Article
                {
                    Id = new Guid(articleRepository.NewId("Id"))
                };
                var resultsBeforeRemove = articleRepository.FindAll();
                Assert.AreEqual(0, resultsBeforeRemove.Count(a => a.Id == article.Id));

                // Act
                articleRepository.Remove(article);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="BaseRepository{T}.Save"/> method.
        /// </summary>
        [TestMethod]
        public void Save_NewEntity_EntitySaved()
        {
            // Arrange
            var count = 10;
            string filePath = null;
            try
            {
                var articles = new List<Article>();
                for (var i = 0; i < count; i++)
                {
                    articles.Add(new Article
                    {
                        ApprovalStatus = 444,
                        ApprovedBy = "Ryan",
                        DateApproved = DateTime.Now,
                        DateTime = DateTime.Now.AddDays(-1),
                        Id = Guid.NewGuid(),
                        Rating = 5,
                        RelatesToId = Guid.NewGuid().ToString(),
                        RelatesToType = "Test",
                        ThreadId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        WebsiteId = Guid.NewGuid()
                    });
                }

                var serialisedArticles = JsonConvert.SerializeObject(articles);
                var serialisedArticleBytes = System.Text.Encoding.Unicode.GetBytes(serialisedArticles);
                filePath = System.IO.Path.Combine(Utilities.IO.UncPath.GetApplicationDirectory(), "[Foundation].[Article].json");
                System.IO.File.WriteAllBytes(filePath, serialisedArticleBytes);
                var articleRepository = new ArticleRepository();

                var newArticle = new Article
                {
                    ApprovalStatus = 444,
                    ApprovedBy = "Ryan",
                    DateApproved = DateTime.Now,
                    DateTime = DateTime.Now.AddDays(-1),
                    Id = Guid.NewGuid(),
                    Rating = 5,
                    RelatesToId = Guid.NewGuid().ToString(),
                    RelatesToType = "Test",
                    ThreadId = Guid.NewGuid(),
                    UserId = Guid.NewGuid().ToString(),
                    WebsiteId = Guid.NewGuid()
                };

                var resultsBeforeSave = articleRepository.FindAll();
                Assert.AreEqual(0, resultsBeforeSave.Count(a => a.Id == newArticle.Id));

                // Act
                articleRepository.Save(newArticle);

                // Assert
                var resultsAfterSave = articleRepository.FindAll();
                Assert.AreEqual(1, resultsAfterSave.Count(a => a.Id == newArticle.Id));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    Utilities.IO.File.Delete(filePath);
            }
        }
    }
}
