﻿using FluentResults;
using FoodPool.post.dtos;

namespace FoodPool.post.interfaces;

public interface IPostService
{
    Task<Result> Create(CreatePostDto createPostDto);
}