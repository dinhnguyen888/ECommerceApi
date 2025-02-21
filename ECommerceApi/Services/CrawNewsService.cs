using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceApi.Models;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Interfaces;

namespace ECommerceApi.Service
{
    public class CrawNewsService : ICrawNewsService
    {
        private readonly IMongoCollection<News> _newsCollection;
        private readonly string baseUrl = "https://vnexpress.net";
        private List<string> sectionList;

        public CrawNewsService(MongoDbContext dbContext)
        {
            sectionList = new List<string> { "/cong-nghe/ai" };
            _newsCollection = dbContext.GetCollection<News>("News");
        }

        public void StartCrawling(int totalCrawlingPage)
        {
            try
            {
                foreach (var section in sectionList)
                {
                    var requestUrl = baseUrl + $"{section}";
                    Console.WriteLine($"Loading URL: {requestUrl}");
                    var document = LoadDocument(requestUrl);
                    Console.WriteLine("Page loaded successfully.");

                    for (var i = 1; i <= totalCrawlingPage; i++)
                    {
                        var requestPerPage = baseUrl + $"{section}-p{i}";
                        Console.WriteLine($"Processing page {i}: {requestPerPage}");
                        var documentForListItem = LoadDocument(requestPerPage);

                        var listNodeProductItem = documentForListItem.DocumentNode.QuerySelectorAll(".item-news.thumb-left.item-news-common");
                        if (listNodeProductItem != null)
                        {
                            foreach (var node in listNodeProductItem)
                            {
                                var newsItem = ParseNewsItem(node, section);

                                if (newsItem != null)
                                {
                                    Console.WriteLine($"Title: {newsItem.Title}");
                                    Console.WriteLine($"Link: {newsItem.LinkDetail}");
                                    Console.WriteLine($"Image: {newsItem.ImageUrl}");
                                    Console.WriteLine("=================================");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No news items found on page {i} for category {section}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during crawling: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public void GetLatestData()
        {
            try
            {
                foreach (var section in sectionList)
                {
                    var requestUrl = baseUrl + $"{section}";
                    var document = LoadDocument(requestUrl);
                    var latestNode = document.DocumentNode.QuerySelector(".item-news.thumb-left.item-news-common:first-child");

                    if (latestNode != null)
                    {
                        var latestNews = ParseNewsItem(latestNode, section);

                        if (latestNews != null)
                        {
                            var existingNews = _newsCollection.Find(n => n.LinkDetail == latestNews.LinkDetail).FirstOrDefault();

                            if (existingNews == null)
                            {
                                _newsCollection.InsertOne(latestNews);
                                Console.WriteLine("Inserted latest data into MongoDB.");

                            }
                            else
                            {
                                Console.WriteLine("The latest data already exists in MongoDB.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No latest news item found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during data retrieval: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private HtmlDocument LoadDocument(string url)
        {
            var web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };
            return web.Load(url);
        }

        private News ParseNewsItem(HtmlNode node, string category)
        {
            var thumbArtNode = node.QuerySelector(".thumb-art > a");
            var thumbImgNode = node.QuerySelector(".thumb-art > a > picture > img");
            var linkDetail = thumbArtNode?.Attributes["href"]?.Value;
            var title = thumbArtNode?.Attributes["title"]?.Value;
            var imgSrc = thumbImgNode?.Attributes["src"]?.Value;
            var description = node.QuerySelector(".description")?.InnerText;

            if (string.IsNullOrEmpty(linkDetail))
            {
                return null;
            }

            // Check if linkDetail is full link or not
            var fullDetailLink = linkDetail.StartsWith("http") ? linkDetail : baseUrl + linkDetail;

            var newsItem = new News()
            {
                Title = title?.RemoveBreakLineTab(),
                LinkDetail = fullDetailLink,
                ImageUrl = imgSrc,
                Type = category,
                Description = description?.RemoveBreakLineTab()
            };

            // Check if the news item already exists in MongoDB
            var existingNews = _newsCollection.Find(n => n.LinkDetail == fullDetailLink).FirstOrDefault();
            if (existingNews == null)
            {
                // If not exists, insert new news item
                _newsCollection.InsertOne(newsItem);
                Console.WriteLine("News item saved to MongoDB.");
            }
            else
            {
                Console.WriteLine("News item already exists. Skipping...");
            }

            return newsItem;
        }

    }
}

public static class StringExtensions
{
    public static string RemoveBreakLineTab(this string input)
    {
        return input.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
    }
}
