﻿using System.Data.Entity;
using ArtigosCientificos.Api.Data;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Models.Review;
using Microsoft.AspNetCore.Mvc;


namespace ArtigosCientificos.Api.Services.Articles
{
    public class ArticleService : IArticleService
    {
        private DataContext _context;
        public ArticleService(DataContext context)
        {
            _context = context;
        }

        public async Task<ObjectResult> CreateArticle(ArticleDTO articleDTO)
        {

            Article article = new Article
            {
                Title = articleDTO.Title,
                Abstract = articleDTO.Abstract,
                Keywords = articleDTO.Keywords,
                File = articleDTO.File,
                UserId = articleDTO.UserId
            };
            var result = await _context.Articles.AddAsync(article);
            if (result == null)
            {
                return new BadRequestObjectResult("Error creating article");
            }
            await _context.SaveChangesAsync();

            //Article createdArticle = _context.Articles.OrderByDescending(a => a.Id).FirstOrDefault();

            Review review = new Review
            {
                ArticleId = article.Id,
                UserId = article.UserId,
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(article);
        }

        public async Task<ObjectResult> GetAllArticles()
        {
            List<Article> articles = _context.Articles.ToList();

            if (articles.Count == 0)
            {
                return new NotFoundObjectResult("No articles found");
            }

            return new OkObjectResult(articles);
        }

        public async Task<ObjectResult> GetLastArticle()
        {
            Article article = _context.Articles.OrderByDescending(a => a.Id).FirstOrDefault();

            if (article == null)
            {
                return new NotFoundObjectResult("No articles found");
            }

            return new OkObjectResult(article);
        }
    }
}
