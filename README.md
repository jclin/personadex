# Personadex

Personadex is a simple Windows Phone 8.1 app that allows the user to scroll through the list of Personas from the [Persona 4](http://en.wikipedia.org/wiki/Shin_Megami_Tensei:_Persona_4) video game. Selecting a Persona brings up a detailed page for it.

Personadex was created as a way to become familiar with the Windows Phone 8.1 APIs. This app adheres to the MVVM pattern, demonstrates data virtualization of a large collection of items, saving ViewModle state for app termination, page navigation, and loading data from a SQLite database.

## Getting Started

Visual Studio 2013 and the Windows Phone 8.1 SDK are required to build the solution.

## MVVM

MVVM is implemented with [MVVM Lite](http://www.mvvmlight.net/).

## Data Virtualization

Data virtualization, or loading items as they come into the visible area of the screen, is accomplished by implementing [IObservableVector](http://msdn.microsoft.com/en-us/library/windows/apps/br226052.aspx). This started off by mimicking a [question on Stack Overflow](http://stackoverflow.com/questions/25784801/virtualization-on-the-viewmodel-layer-in-winrt).

## Transient State

Saving and restoring transient state is normally handled with the `NavigationHelper` and `SuspensionManager` classes that Microsoft provides. However, I opted to roll my own solution as they rely on logic in the view layers, whereas I wanted to maintain a "ViewModel first" approach.

## iOS and Android

Personadex currently is only implemented on WinPhone 8.1, but there are plans to port it to iOS and Android after I learn Ruby and RubyMotion :-]

## Legal Stuff

I am in no way affiliated with ATLUS, the developer of the Persona video game series.
