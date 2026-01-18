---
title: Code Review Instructions
description: Guidelines and best practices for reviewing code in this repository.
applyTo: "**"
scope: workspace
---

# Code Review Instructions

## Purpose

Provide clear, actionable steps and standards for code review to ensure code quality, maintainability, and security.

## Instructions

- Review code for readability, maintainability, and adherence to project conventions.
- Ensure all changes are covered by appropriate tests.
- Check for security vulnerabilities and follow secure coding practices.
- Confirm that documentation is updated as needed.
- Use the code-review-checklist.skill.md for detailed review points.

## References

- See the main README.md for project overview.
- Refer to the code-review-checklist.skill.md for a detailed checklist.

## Testing & Documentation

- You must include appropriate unit/integration tests for new features.
- You must provide XML comments or docstrings for public APIs.
- You must update `README.md` or feature docs if behavior changes.

## General Review Checklist

- You must ensure code is clear, readable, and maintainable.
- You must follow project folder structure and naming conventions.
- You must never include hardcoded secrets or credentials.
- You must never leave commented-out or dead code.
- You must ensure all new/changed code is covered by tests.
- You must follow all referenced instruction files in `.github/instructions/`.

---

For more details, you must see the referenced instruction files in `.github/instructions/` and the main `README.md`.

## 6. Testing & Documentation

- New features must include appropriate unit/integration tests.
- Public APIs should have XML comments or docstrings.
- Update `README.md` or feature docs if behavior changes.

## 7. General Review Checklist

- [ ] Code is clear, readable, and maintainable
- [ ] Follows project folder structure and naming conventions
- [ ] No hardcoded secrets or credentials
- [ ] No commented-out or dead code
- [ ] All new/changed code is covered by tests
- [ ] Follows all referenced instruction files in `.github/instructions/`

---

For more details, see the referenced instruction files in `.github/instructions/` and the main `README.md`.
