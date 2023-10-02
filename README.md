# easystub

A simple REST API configurable with a web application

:warning: **THIS IS A WORK IN PROGRESS** :warning:

The goal of this project is to have a highly configurable Node.JS Express REST API. You can define in a web application what are the routes you want to define and what these routes should return.

In other words, this project is to help people making isolated manual tests on JSON APIs. If you develop an app that consumes another API and you want to test some very precise corner-cases without bothering the other team, just stub them!

This will all be containerized into one single Docker image for ease of use.

## The stack

API built with NodeJS & Express

Web app built with Blazor **WASM** (new to me, this is the first project I'm doing with this tech)
I'm sorry if the code is not uniform but I wanted to discover and test multiple things with Blazor and MudBlazor.

## Run locally

Simply use `docker-compose up` and the UI and API containers will be running. Simply open [http://localhost](http://localhost) and all should be working.

## Run on a server

I'm still not there yet!
