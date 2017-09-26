# Object Relational Mapped Monitor Extension

### Model-View-Controller / Adapter Dynamically Linked Library ([MVC]/A [DLL])

---

## Task-based Asynchronous Pattern ([TAP])

* CREATED BY:   [Latency McLaughlin]
* FRAMEWORK:    [.NET] v[4.6.2](https://www.microsoft.com/en-us/download/details.aspx?id=53345)
* LANGUAGE:     [C#] (v6.0)
* GFX SUBSYS:   [WPF]
* SUPPORTS:     [Visual Studio] 2017, 2015, 2013, 2012, 2010, 2008
* UPDATED:      3/2/2017
* VERSION:      [2.1.3](https://www.nuget.org/packages/ORM-Monitor/2.1.3/)
* TAGS:         [API], [TAP], [TPL], [ORM], [MVC], [AMI], [.NET], [C#], [WPF], [Parametric Polymorphism]

### Screenshot
 <img src="https://github.com/Latency/ORM-Monitor/blob/master/ORM-Monitor.png?raw=true">

<hr>

## Navigation
* <a href="#background">Background</a>
* <a href="#introduction">Introduction</a>
* <a href="#overview">Overview</a>
* <a href="#installation">Installation</a>
* <a href="#using">Using the code</a>
* <a href="#other">Other features</a>
* <a href="#references">References</a>
* <a href="#license">License</a>

<hr>

<h2><a name="background">Background</a></h2>

There have been many instances throughout my career where incompetent developers try to release code to the company and its stakeholders which deadlock underlying dependency OSI layers which hinder project performance and scheduling.
Those tasked to sustain code as dependency will be frustrated debugging these efforts when trying to qualify responsibility of failure.

The goal was to develop an [API] that would be universal and simple to use in helping eliminate any '<i>concerns</i>' a.k.a cohesive areas of functionality, by wrapping amateur developers efforts with ease and addressing cross-cutting/horizontal concerns as well.
The goal was to reduce time & development costs in validating efforts for risk mitigation serving as a universal multi-functional paradigm.
Any developer can therefore use it to construct new code with the same underlying mechanics and reduce boilerplate code.

When we reference an <i>Object Relational Mapped</i> sub-system it means it is polymorphic and is generic by convention that doesn't depend on specific references to types.  Think of it as a universal template which can inject the types in question as its specified type parameter and let any of the mapping be done under the hood in a <i>dynamic</i> manor.

Some nice things about the C# language is that it is far superior to Java and can detect within a certain tolerance what the type might be so that the type parameter can be omitted from its calling convention.
Also, that both languages have Reflection built-in so it is possible to be able to drill down many OSI layers and pull out information used for dynamic / RT calling conventions.

Java has limitations and has nothing to offer of greater value which is described here for [Comparison].
In fact, our methodology for this article is to use '<i>generic programming</i>'; a feature set of Java not properly support since reflection cannot be used to construct new generic realizations nor allows generics directly for primitive types.

<h2><a name="introduction">Introduction</a></h2>

This article introduces an [API] which wraps processes asynchronously; supporting deadlock protection, timeout, cancellation requests, checkpointing, and a parametric polymorphic [MVC] design by convention.

Callback support for the following delegates:
* OnCompleted
* OnTimeout
* OnExit
* OnProgressChanged
* OnRunning
* OnCancellation

[ORM-Monitor] is compiled as a library packaged for the [NuGet] marketplace.  The project itself, supplies an application driver, test suite, and the library's source code.

Model-View-Controller ([MVC]) is an architectural pattern that separates application's data model and user interface views into separate components. This is what the definition says. However, to fully understand the [MVC] we have to introduce required terms and point benefits of [MVC].

<p align="center">
 <img src="http://www.ii.uni.wroc.pl/~wzychla/images/mvc.png" alt="A wiki image of MVC">
</p>

The view is a user interface element (typically a window) that presents the data to the user and allows the user to change the data. It is a typical situation to have several views active at the same time (for example in the Multiple-Document Interface).

The controller is a connection point between the model and views: views register in the controller and reports changes in the model to them.

The data model is a set of data structures that lay the base for the businnes logic of an application. In typical object-oriented application, the data model is built of client-side classes and collections. The data model typically is somehow stored into a Database Management System, however how the data is exactly stored is not a concern of [MVC].  In fact, other than speed in it's performance, models stored within the DBMS is not as efficient as an ORM based one.
There are many references for this and it only makes sense.   Injection efforts and security risks are not of a concern as old myths die hard dictate.   Dynamic and parametric polymorphic efforts show that RT constructs perform with much better flexibility and tightly coupled integration within a projects design.
Future efforts for extension and modification can be reduced and even eliminated when used in conjunction with a reflective based programming language.

So you have to ask yourself, why would we use deprecated features such as stored procedures to construct a data model integrating in all the business logic which is much more difficult to implement and maintain, when there are options to construct a more dynamic, flexible, easier to maintain, and understand model?
There are alternatives which are far superior; such as using an ORM sub-system.

-- Disadvantages
1. A DBA will be required for performance tuning
2. All developers will have to be very well versed in your particular SQL dialect(T-SQL, Pl/SQL, etc)
3. SQL code isn't as expressive and thus harder to write when covering higher level concepts that aren't really related to data
4. A lot more unnecessary load on the database

Practically, only a fool would have all business logic in the database.

1. Very few developers will be able to create a consistent stored procedure interface that works easily across applications. Usually this is because certain assumptions are made of that calling application
2. Same goes for documenting all of those stored procedures
3. Database servers are generally bottlenecked enough as it is. Putting the unnecessary load on them just further narrows this bottleneck. Intricate load balancing and beefy hardware will be required for anything with a decent amount of traffic
4. SQL is just barely a programming language. I once had the pleasure of maintaining a scripting engine written as a T-SQL stored procedure. It was slow, nearly impossible to understand, and took days to implement what would have been a trivial extension in most languages
5. What happens when you have a client that needs their database to run a different SQL server? You'll basically have to start from scratch -- You're very tied to your database. Same goes for when Microsoft decides to deprecate a few functions you use a couple hundred times across your stored procedures
6. Source control is extremely difficult to do properly with stored procedures, more so when you have a lot of them
7. Databases are hard to keep in sync. What about when you have a conflict of some sort between 2 developers that are working in the database at the same time? They'll be overwriting each others code not really aware of it, depending on your "development database" setup
8. The tools are definitely less easy to work with, no matter which database engine you use.

Forget about the data model and focus on replacing this with an adapter.  Model–view–adapter [MVA] or mediating-controller [MVC] is a software architectural pattern and multitier architecture.
The model–view–adapter solves this rather differently from the model–view–controller by arranging model, adapter or mediating controller and view linearly without any connections whatsoever directly between model and view.
What do you call something that has connections directly related?   Dependancies!  If a dependancy breaks down, so does the rest of the system.  Think about it!  In an alternative scenario, you can use [DI] to replace additional constraints that may be binded as well.   This is where aspect oriented programming ([AOP]) will reduce the amount of boilerplate code which in effect also reduces the maintence efforts and cost along with the project size and complexity.
<!--
 This is what many companies fail to understand and I have been passed over several times by hiring managers because they don't even have a clue or simpily don't want to look bad knowing that you could take their job and would never admit to feeling intimidated.  Companies want what they want, use what they have currently in place to sustain their development efforts, and not have to re-train their staff to use more efficient designs.  Do you really want to work for a company who is complacent in their way of doing things anyway?    Is it cost effective for the company to use efforts that are behind the curve?  Eventually there comes a time where continous integration and patching dictate a rewrite or redesign.   Why prolong efforts when eventually it will need to be done?   Get with the times ... for real!!!  Their support staff may be using efforts that work but are not as efficient as alternative methodologies being proposed here.
-->

---

This API has several benefits, such as:

-- *[ORM-Monitor] helps you to know exactly what your code does.*
* Removes pitfalls, anti-patterns, and programming mistakes.
* Eliminate code redundancy.
* Reusable functional procedural code.
* Decrease RAD time.
* Optomizes performance of asynchronous code.
* Fix performance problems easily.
* Inject delegates in real time.
* Reduce boilerplate code.

-- *Flexible delegate injection for any situation.*
* Dependancy injection:<br>
  Design patterns that implement inversion of control for resolving dependencies.
* Labmda expressions:<br>
  An anonymous function that you can use to create delegates or expression tree types and supersede anonymous methods for writing inline code.
* Anonymous methods:<br>
  Constructs a way to pass a code block as a delegate parameter used in building up the *TaskEvent* class.
* Generic types:<br>
  Independant type parameters act as a placeholder for a specific type that a client specifies when they instantiate a variable of the generic type.

<h2><a name="overview">Overview</a></h2>

* <b>Graphical user interface samples.</b><br>
 A clever user-interface is included which offers a view to spawn and cancel tasks.

* <b>Async / Await support.</b><br>
 Task monitoring and dispatching utilizes [TPL] found in [.NET]'s v4+ async [API] which is fully supported by every task is wrapped and invocated based on its delegate.  Tightly coupled integration of multi-threaded delegates with composite [TPL] mechanics to offer thread-safe immediate and explicit timeout and termination functionality.   

* <b>Create reusable parametric polymorphic / [lambda expression] / FP methods in code.</b><br>
 When running your tests, it might be that you want to create a sample of functional or regressive tasks.  Interchanging the delegates make it easy.  For example, switching contexts from an administrator area in your [CMS] / web-application.

* <b>Multiple O/R mapper framework concurrent scheduling.</b><br>
 [ORM-Monitor] uses a wrapping factory for the delegates supplied as its generic type. It doesn't matter whether your application uses multiple event handlers with specific listeners for each.  This [API] will consolidate those into a universal system regardless of type.   It will reduce the boilerplate code and overhead which will ultimately reduce the overall risk of failure since everything will funnel through the same sub-system.

<h2><a name="installation">Installation</a></h2>

This library can be installed from [NuGet]:

There are plug-ins that this project uses as dependency from [NuGet] that are built-in and merged into the [API].


<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="2">Assembly - GUI</th>
  </tr>
    <tr>
    <td>External References</td>
    <td><i>ORM-Monitor</i> - v2.1.3</td>
    <td><i>Microsoft.ExceptionMessageBox</i> - v11.0.2100.60</td>
  </tr>
  <tr>
    <td>Exception logging & UI application hooks</td>
    <td><i>ReflectSoftware.Insight</i> - v5.6.1.2</td>
    <td><i>Newtonsoft.Json - JSON.NET</i> - v9.0.1</td>
  </tr>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="2">Assembly - DLL</th>
  </tr>
  <td>API</td>
  <td><i>ORM-Monitor</i> - v2.1.3</td>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;">Assembly - Tests</th>
  </tr>
  <tr>
    <td>External References</td>
    <td><i>ORM-Monitor</i> - v2.1.3</td>
  </tr>
  <tr>
    <td>Unit Testing</td>
    <td><i>NUnit</i> - v3.6.1</td>
  </tr>
</table>


<h2><a name="using">Using The Code</a></h2>

There are four essential steps to using this:

&nbsp;1. Optional:<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Setup a `TaskEventArgs<T>.Expression` delegate source with ... parameters.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Make any <i>delegate</i> use the expression, which is basically a wrapper for invocating a method across<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; boundary switching thread contexts.

```csharp
   TaskEventArgs<MyType>.Expression expression = args => {
     var ctl = args[0] as Control;
     var a   = args[1] as MethodInvoker;
     ...
     if (ctl.InvokeRequired)
       ctl.Invoke(a);
     else
       a();
     ...  
     return default(MyType);  
   };
```

&nbsp;2. Create a \`<i>TaskEvent</i>\` with type parameter matching the type from #1. (if specified)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Using <i>object initialization</i>, specify the timeout for 5 seconds which is shorter than the default.<br>
&nbsp;3. Specify the `OnRunning` event handler delegate.  (<b>Required</b>)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;There is an exception handler for this in the \`<i>AsyncMonitor</i>\` method if omitted.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i><b>Optional</b></i>:<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Construct the `OnExit`, `OnProgressChanged`, `OnCanceled`, `OnCompleted`, and `OnTimeout` event handler delegates similarly.<br>
&nbsp;4. Invoke the extension method.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>[TaskEvent.cs]</i>:   Extension methods for asynchronous routines.

   Usage:
```csharp
   //<Task> = <TaskEvent>.AsyncMonitor();
   Task task = @t0.AsyncMonitor();
```
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Or since it is an extension method, it can be called explicitly since static:
```csharp
   Async_IO.AsyncMonitor(@t0);
```

### Example #1
```csharp
    TaskEventArgs<MyType>.Expression expression = args => {
      var obj = args[0];
      var str = args[1] as string;
      var testNo = obj != null ? $"{((TaskEvent<MyType>) obj).Name}: " : string.Empty;
      Debug.WriteLine(testNo + str);
      return default(MyType);
    };
    
    MyType canceled = id => $"Task ID({id}) has canceled.";
      
    var @t0 = new TaskEvent<MyType>(expression, source: canceled, timeout: TimeSpan.FromSeconds(5)) {
      Name = "t0"
    };

    @t0.OnRunning((th, tea) => {
      SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 2));
    });

    @t0.OnCompleted((th, tea) => {
      tea?.Invoke(@t0, Messages.Completed);
    });

    @t0.OnTimeout((th, tea) => {
      tea?.Invoke(@t0, Messages.Timeout);
      Assert.Fail("timeout @t0");
    });

    @t0.OnCanceled((th, tea) => {
      tea?.Invoke(@t0, tea.Source.Invoke(tea.Event.Task.Id));
      Assert.Fail("canceled @t0");
    });

    @t0.OnExit((th, tea) => {
      tea?.Invoke(@t0, Messages.Exited);
    });

    // The extension method to use.
    Task task = @t0.AsyncMonitor();
```

1. Run \`<i>TaskScheduler.exe</i>\`.
2. Click the `Start New Task` button in the window pane to spawn a new event.
3. Click the `Cancel` button to stop the event.
4. Click the `Remove` button to remove the row from the list.

<h2><a name="other">Other Features</a></h2>

- Unit Tests:<br>
  The unit test uses [NUnit] to help qualify the underlying [API].   Included is a sample that can be ran and tested againsted a variety of mock senario conditions.
- UI Application:<br>
  In addition to being able to use the [API] library as standalone, the project consists of a user-interface I developed using [WPF].

<h2><a name="references">References</a></h2>

 [TPL], [.NET], [ORM], [IoC], [DI], [Generics], [Delegates], [EventHandlers], [Parametric Polymorphism]

<h2><a name="license">License</a></h2>

[GNU LESSER GENERAL PUBLIC LICENSE] - Version 3, 29 June 2007


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job.)

   [GNU LESSER GENERAL PUBLIC LICENSE]: <http://www.gnu.org/licenses/lgpl-3.0.en.html>
   [Comparison]: <https://en.wikipedia.org/wiki/Comparison_of_C_Sharp_and_Java>
   [TaskEvent.cs]: <https://github.com/Latency/ORM-Monitor/blob/master/DLL/TaskEvent.cs>
   [NuGet]: <https://www.nuget.org/packages/ORM-Monitor/>
   [.NET]: <https://en.wikipedia.org/wiki/.NET_Framework/>
   [WPF]: <https://en.wikipedia.org/wiki/Windows_Presentation_Foundation/>
   [Visual Studio]: <https://en.wikipedia.org/wiki/Microsoft_Visual_Studio/>
   [Latency McLaughlin]: <https://www.linkedin.com/in/Latency/>
   [API]: <https://en.wikipedia.org/wiki/Application_programming_interface>
   [AOP]: <https://en.wikipedia.org/wiki/Aspect-oriented_programming>
   [Parametric Polymorphism]: <https://en.wikipedia.org/wiki/Parametric_polymorphism>
   [ORM-Monitor]: <https://github.com/Latency/ORM-Monitor/>
   [TAP]: <https://msdn.microsoft.com/en-us/library/hh873175(v=vs.110).aspx>
   [AMI]: <https://en.wikipedia.org/wiki/Asynchronous_method_invocation>
   [TPL]: <https://msdn.microsoft.com/en-us/library/dd460717(v=vs.110).aspx>
   [ORM]: <https://en.wikipedia.org/wiki/Object-relational_mapping>
   [C#]: <https://en.wikipedia.org/wiki/C_Sharp_(programming_language)>
   [DLL]: <https://en.wikipedia.org/wiki/Dynamic-link_library>
   [MVC]: <https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller>
   [MVA]: <https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93adapter>
   [CMS]: <https://en.wikipedia.org/wiki/Content_management_system>
   [IoC]: <https://msdn.microsoft.com/en-us/library/ff921087.aspx>
   [DI]: <https://en.wikipedia.org/wiki/Dependency_injection>
   [Generics]: <https://en.wikipedia.org/wiki/Generic_programming>
   [Delegates]: <https://msdn.microsoft.com/en-us/library/ms173171.aspx>
   [EventHandlers]: <https://msdn.microsoft.com/en-us/library/2z7x8ys3(v=vs.90).aspx>
   [NUnit]: <https://en.wikipedia.org/wiki/NUnit>
   [lambda expression]: <https://msdn.microsoft.com/en-us/library/bb397687.aspx>