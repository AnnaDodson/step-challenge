# Contributing

Please contribute! 

If this is your first time, follow the steps listed below. Or if you're familiar with the process, TLDR: Fork the repository, commit your changes then raise a PR! If you're closing an issue, mention the issue number in the branch name and PR.

## Contents
   1. [Fork and Clone the Repo](#fork-and-clone-the-repo)
   2. [Raising Your Pull Request](#raising-your-pull-request)


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

$ git checkout develop

$ git merge upstream/develop

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

## Raising Your Pull Request

Once you've created your blog post, fixed an issue or added a feature it's time to commit your changes and raise a pull request.

To make a commit, first check what files you've changed or added:

```
$ git status
```

You'll see all the files you've either edited or added.

Before committing, it's always good to check you're on the right branch.

```
$ git branch
```

If you want to switch branch:

```
$ git checkout <branch-name>
```

Add `-b` after `checkout` if you want to make a new branch. Then to commit the files you want, add them by running:

```
$ git add <files that needing adding>

// or to add them all, you can run this

$ git add .
```

Then commit:
```
$ git commit -m "Adding <descrption here>"
```

You can enter any other details that you think are necessary too.

Once you've committed, you push!
```
$ git push origin <your-branch-name>
```

Also see [here](https://help.github.com/articles/adding-a-file-to-a-repository-using-the-command-line/) for more information.

If you go over to GitHub in your browser, you should see a flag appear in your fork about raising a PR - click that and follow the instructions. Don't forget to add a good message explaining what you have done in your merge. [More details here](https://help.github.com/articles/creating-a-pull-request/)

If at any point you're confused or lost, speak up and get in touch!! You can respond to an issue if you're working on one or email me, tweet me or find me on slack. Don't give up or keep quiet, I want to help.
