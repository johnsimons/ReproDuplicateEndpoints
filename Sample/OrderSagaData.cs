using System;
using System.Collections.Generic;
using NServiceBus.Saga;

public class OrderSagaData : IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
    
    [Unique]
    public int OrderId { get; set; }
    public string Data { get; set; }
    public int ReallyLongNameOfPropertyToShowSomeBusWithReallyLongNamesToFigureOutHowToDealWithIt { get; set; }
    public List<LineItem> LineItems { get; set; }
}

public class LineItem
{
    public string Name;
    public int Count;
}