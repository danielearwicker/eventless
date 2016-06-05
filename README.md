eventless
=========

Inspired by the knockout.js pattern(s), in C#, blogging as I go at http://smellegantcode.wordpress.com/2013/02/25/eventless-programming-part-1-why-the-heck/

*Update* 2016-06-05 - I've kept the original WinForms code in a branch called `winforms`.
The master branch is now modified to target WPF. The major differences are:

The `INotifyPropertyChanged` interface is inherited by `IGetable<T>`, and it replaces
the removed non-generic `IGetable`. It always specifies `Value` as the name string in
the `PropertyChanged` event. This means that any getable is automatically ready for WPF
two-way binding, e.g.:

    <TextBox Text="{Binding NewNoteText.Value, Mode=TwoWay}"/>

Note that you have to bind to the `Value` property.

Similarly, `SetableList` is now replaced with a much simpler `GetableList` that simply
acts as a wrapper around `ObservableCollection`, and integrates it with the automatic
dependency-tracking of `Computed`.

The `Computed` feature is changed to work like `ko.pureComputed`, which makes it much
better at automatically cleaning up: when the last listener on its `PropertyChanged`
event delists, it delists from `PropertyChanged` on all its own dependencies. You can
still force this cleanup to occur by calling `Dispose`.

I've also removed a few things for now, until I figure out a tidy way to do them,
such as `AsyncComputed`.

MIT License
===========

https://opensource.org/licenses/MIT

Copyright (c) 2013-2016 Daniel Earwicker <dan@earwicker.com>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
