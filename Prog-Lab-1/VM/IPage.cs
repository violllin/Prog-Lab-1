using System.Collections;

namespace VM.Domain;

public interface IPage
{
    int PageIndex { get; }
    
    DateTime AccessTime { get; set; }

    bool IsModified { get; set; }
    
    BitArray BitMap { get; }
    
    int[] Data { get; }
}