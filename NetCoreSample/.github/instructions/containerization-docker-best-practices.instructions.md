---
title: Containerization & Docker Best Practices
description: Best practices for Docker and containerization in this project.
applyTo: "**"
scope: workspace
---

# Containerization & Docker Best Practices

## Purpose

Ensure Docker images and containers are secure, efficient, and production-ready.

## Instructions

- Use multi-stage builds to minimize image size.
- Exclude unnecessary files using .dockerignore.
- Do not run containers as root; use a non-root user.
- Keep images up to date and pin versions where possible.
- Clean up build artifacts and temporary files in final images.
- Document exposed ports and environment variables.
- Follow the official Dockerfile and docker-compose.yml conventions.

## References

- See README.md for build and deployment steps.
