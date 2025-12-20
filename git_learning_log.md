# Git Learning Log - NonaWin Project

This document records the Git commands and concepts used to manage the NonaWin project.

## 1. Initial Setup (Initializing the Repository)

To start version controlling an existing project, we use the following commands:

### Commands Used:
```bash
# Initialize a new Git repository in the current directory
git init

# Create a .gitignore file (crucial for .NET projects to ignore build artifacts)
# (We created this file manually, but it tells git what NOT to track)

# Stage all files for the first commit
git add .

# Create the initial commit
git commit -m "Initial commit: NonaWin v1.0 with Explorer UI and Duplicate Analysis"
```

### Concept: The Staging Area
- **`git init`**: Creates a hidden `.git` folder that tracks changes.
- **`git add`**: Moves changes from your working directory to the "Staging Area" (ready to be saved).
- **`git commit`**: Takes a snapshot of the Staging Area and saves it to the history.

---

## 2. Feature Development Workflow (Branching)

**Question**: *Should I modify directly or use a branch?*

**Best Practice**: Use **Branches**.
Even if you are working alone, using branches helps you organize your thoughts and allows you to "switch context" or discard failed experiments easily.

### Recommended Workflow:
1.  **Create a new branch** for the feature:
    ```bash
    git checkout -b feature/your-feature-name
    ```
2.  **Make changes** and test.
3.  **Commit changes** to that branch.
4.  **Merge** back to the main branch when done.

### Commands for Branching:
```bash
# List all branches
git branch

# Create and switch to a new branch
git checkout -b feature/new-ui-color

# Switch back to the main branch
git checkout master  # or main

# Merge the feature branch into the current branch
git merge feature/new-ui-color
```
