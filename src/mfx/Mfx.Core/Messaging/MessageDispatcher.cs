// =============================================================================
//               __
//              / _|
//    _ __ ___ | |___  __
//   | '_ ` _ \|  _\ \/ /
//   | | | | | | |  >  <
//   |_| |_| |_|_| /_/\_\
//
// MIT License
//
// Copyright (c) 2024 Sunny Chen (daxnet)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// =============================================================================

namespace Mfx.Core.Messaging;

public sealed class MessageDispatcher
{
    #region Private Fields

    private readonly Dictionary<Type, List<Action<object, IMessage>>> _handlers = new();

    #endregion Private Fields

    #region Public Methods

    public void Dispatch<TMessage>(object publisher, TMessage message)
        where TMessage : IMessage
    {
        if (_handlers.TryGetValue(typeof(TMessage), out var actionList))
            Parallel.ForEach(actionList, action => action(publisher, message));
    }

    public void RegisterHandler<TMessage>(Action<object, TMessage> handler)
        where TMessage : IMessage
    {
        Action<object, IMessage> action = (publisher, message) => handler(publisher, (TMessage)message);

        if (_handlers.TryGetValue(typeof(TMessage), out var actionHandlers))
            actionHandlers.Add(action);
        else
            _handlers.TryAdd(typeof(TMessage), [action]);
    }

    #endregion Public Methods
}