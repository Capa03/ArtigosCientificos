using ArtigosCientificosMvc.Service.Api;
using ArtigosCientificosMvc.Service;
using ArtigosCientificosMvc.Models.Review;
using ArtigosCientificosMvc.Service.Review;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class ReviewService : IReviewService
{
    private readonly ApiService apiService;
    private readonly ConfigServer configServer;

    public ReviewService(ApiService apiService, ConfigServer configServer)
    {
        this.apiService = apiService;
        this.configServer = configServer;
    }

    public async Task<List<ReviewWithArticleDTO>> GetAllReviewsByStatus(string Status)
    {
        try
        {
            // Get the reviews and the associated articles by joining them on ArticleId
            List<ReviewWithArticleDTO> reviewsWithArticles = await this.apiService.GetTAsync<List<ReviewWithArticleDTO>>(configServer.GetReviewsUrl(Status));

            if (reviewsWithArticles == null || !reviewsWithArticles.Any())
            {
                return null;
            }

            return reviewsWithArticles;
        }
        catch (Exception ex)
        {
            // Log the error or rethrow to be handled by the caller
            throw new Exception($"Error fetching reviews with status '{Status}': {ex.Message}", ex);
        }
    }

    public async Task<ReviewWithArticleDTO> GetReviewById(int id)
    {
        try
        {
            ReviewWithArticleDTO review = await this.apiService.GetTAsync<ReviewWithArticleDTO>(configServer.GetReviewsByIdUrl(id));

            if (review == null)
            {
                return null;
            }
            return review;

        }
        catch (Exception ex)
        {
            // Log the error or rethrow to be handled by the caller
            throw new Exception($"Error fetching review with id '{id}': {ex.Message}", ex);
        }
    }

    public async Task<ObjectResult> UpdateReview(int id, ReviewPutDTO review)
    {
        try
        {
            var (data, statusCode) = await this.apiService.PutTAsync<ReviewPutDTO>(configServer.PUTReviewsByIdUrl(id), review);

            if (statusCode == HttpStatusCode.OK && data != null)
            {
                return new ObjectResult(data)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                return new ObjectResult(new { Message = $"Review with id '{id}' not found." })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            else
            {
                return new ObjectResult(new { Message = "Failed to update review. Please check the input data." })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
        catch (Exception ex)
        {
            // Log or return an error response
            return new ObjectResult(new { Message = $"Error updating review with id '{id}': {ex.Message}" })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

}
