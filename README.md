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
* VERSION:      [2.1.1](https://www.nuget.org/packages/ORM-Monitor/2.1.1/)
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
There have been many instances throughout my career where incompetent developers try to release code to the company and its stakeholders which deadlock underlying dependency OSI layers which hinder project performance and scheduling.  Those tasked to sustain code as dependency will be frustrated debugging these efforts when trying to qualify responsibility of failure.

The goal was to develop an API that would be universal and simple to use in helping eliminate any concerns by wrapping amateur developers efforts with ease.
The goal was to reduce time & development costs in validating efforts for risk mitigation serving as a universal multi-functional paradigm.
Any developer can therefore use it to construct new code with the same underlying mechanics and reduce boilerplate code.

When we reference an <i>Object Relational Mapped</i> sub-system it means it is polymorphic and is generic by convention that doesn't depend on specific references to types.  Think of it as a universal template which can inject the types in question as its specified type parameter and let any of the mapping be done under the hood in a <i>dynamic</i> manor.

Some nice things about the C# language is that it can detect within a certain tolerance what the type might be so that the type parameter can be omitted from its calling convention.
Also, that the language has Reflection built-in so we can drill down to many layers and pull out information used for dynamic / RT calling conventions.

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

The data model is a set of data structures that lay the base for the businnes logic of an application. In typical object-oriented application, the data model is built of client-side classes and collections. The data model typically is somehow stored into a Database Management System, however how the data is exactly stored is not a concern of [MVC].

The view is a user interface element (typically a window) that presents the data to the user and allows the user to change the data. It is a typical situation to have several views active at the same time (for example in the Multiple-Document Interface).

The controller is a connection point between the model and views: views register in the controller and reports changes in the model to them.

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
 [ORM-Monitor] Extension uses a wrapping factory for the delegates supplied as its generic type. It doesn't matter whether your application uses multiple event handlers with specific listeners for each.  This [API] will consolidate those into a universal system regardless of type.   It will reduce the boilerplate code and overhead which will ultimately reduce the overall risk of failure since everything will funnel through the same sub-system.

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
    <td><i>ORM-Monitor</i> - v2.1.1</td>
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
  <td>N/A</td>
  <td/>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;">Assembly - Tests</th>
  </tr>
  <tr>
    <td>External References</td>
    <td><i>ORM-Monitor</i> - v2.1.1</td>
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

    @t0.OnRunning((obj, tea) => {
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
   [TaskEvent.cs]: <https://github.com/Latency/ORM-Monitor/blob/master/DLL/TaskEvent.cs>
   [NuGet]: <https://www.nuget.org/packages/ORM-Monitor/>
   [.NET]: <https://en.wikipedia.org/wiki/.NET_Framework/>
   [WPF]: <https://en.wikipedia.org/wiki/Windows_Presentation_Foundation/>
   [Visual Studio]: <https://en.wikipedia.org/wiki/Microsoft_Visual_Studio/>
   [Latency McLaughlin]: <https://www.linkedin.com/in/Latency/>
   [API]: <https://en.wikipedia.org/wiki/Application_programming_interface>
   [Parametric Polymorphism]: <https://en.wikipedia.org/wiki/Parametric_polymorphism>
   [ORM-Monitor]: <https://github.com/Latency/ORM-Monitor/>
   [TAP]: <https://msdn.microsoft.com/en-us/library/hh873175(v=vs.110).aspx>
   [AMI]: <https://en.wikipedia.org/wiki/Asynchronous_method_invocation>
   [TPL]: <https://msdn.microsoft.com/en-us/library/dd460717(v=vs.110).aspx>
   [ORM]: <https://en.wikipedia.org/wiki/Object-relational_mapping>
   [C#]: <https://en.wikipedia.org/wiki/C_Sharp_(programming_language)>
   [DLL]: <https://en.wikipedia.org/wiki/Dynamic-link_library>
   [MVC]: <https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller>
   [CMS]: <https://en.wikipedia.org/wiki/Content_management_system>
   [IoC]: <https://msdn.microsoft.com/en-us/library/ff921087.aspx>
   [DI]: <https://en.wikipedia.org/wiki/Dependency_injection>
   [Generics]: <https://en.wikipedia.org/wiki/Generic_programming>
   [Delegates]: <https://msdn.microsoft.com/en-us/library/ms173171.aspx>
   [EventHandlers]: <https://msdn.microsoft.com/en-us/library/2z7x8ys3(v=vs.90).aspx>
   [NUnit]: <https://en.wikipedia.org/wiki/NUnit>
   [lambda expression]: <https://msdn.microsoft.com/en-us/library/bb397687.aspx>