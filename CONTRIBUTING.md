# Contributing

Please contribute! 

If this is your first time, follow the steps listed below. Or if you're familiar with the process, TLDR: Fork the repository, commit your changes then raise a PR! If you're closing an issue, mention the issue number in the branch name and PR.

## Contents
1. [Code Contributions Through Git](#code-contributions-through-git)
   1. [Fork and Clone the Repo](#fork-and-clone-the-repo)
   2. [Adding a Blog Post](#adding-a-blog-post)
   2. [Raising Your Pull Request](#raising-your-pull-request)


## Code Contributions Through Git

## Fork and Clone the Repo

First, follow this [step by step guide](https://guides.github.com/activities/forking/) to fork the repository and then clone it so you have a copy on your local computer.

If you've never used GitHub before, you'll need to add ssh keys to communicate to GitHub from your computer. Full steps [here](https://help.github.com/articles/adding-a-new-ssh-key-to-your-github-account/).

Make sure you have your keys set up before continuing.

Next, open a terminal wherever you want the project directory to be and run:

```
$ git clone https://github.com/<your-username-goes-here>/step-challenge/
```

Replace the URL with your own repository URL path. If you're not sure what it is, go to GitHub where you forked the project and check your repository home page for the full URL.

Once that's cloned, cd into the new project:

```
$ cd step-challenge/
```

To make sure your fork knows which is the upstream repository, run:

```
$ git remote add upstream git@github.com:AnnaDodson/step-challenge.git
```

This means if there are any changes to the main branch you can get them by running the following:

``` 
$ git fetch upstream

$ git checkout master

$ git merge upstream/master

 ```

It's good practice to do this often and make sure your branch is up to date before raising your PR.

If you ever want to check your origin and upstream, you can run the following:

```
$ git remote -v 
```

There are some more in-depth instructions [here](https://help.github.com/articles/fork-a-repo/) that you can take a look at if you want some further reading.

To make your changes, you'll want to create a new branch to work on. Use a descriptive branch name with no spaces, capital letters or special characters. If you're working on an issue, mention the issue number.

```
$ git checkout -b bugfix/fix-annas-janky-code
```

Woohoo, you're all ready! Follow the instructions in the README to get the site up and running locally.
