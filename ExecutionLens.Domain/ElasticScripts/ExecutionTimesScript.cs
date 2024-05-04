namespace ExecutionLens.Domain.Scripts;

public class ExecutionTimesScript
{
    public const string Name = nameof(ExecutionTimesScript);

    public const string Init = @"state.durations = []";

    public const string Map = @"state.durations.add(doc['exitTime'].value.getMillis() - doc['entryTime'].value.getMillis());";

    public const string Combine = @"double min = Double.POSITIVE_INFINITY; 
                                    double max = Double.NEGATIVE_INFINITY; 
                                    double sum = 0;
                                    for (t in state.durations) {
                                        min = Math.min(min, t);
                                        max = Math.max(max, t);
                                        sum += t;
                                    }
                                    return [ 'min': min, 
                                             'max': max, 
                                             'avg': sum / state.durations.size(), 
                                             'sum': sum, 
                                             'count': state.durations.size() ];";

    public const string Reduce = @"double min = Double.POSITIVE_INFINITY; 
                                   double max = Double.NEGATIVE_INFINITY; 
                                   double sum = 0; long count = 0;
                                   for (a in states) {
                                       min = Math.min(min, a['min']);
                                       max = Math.max(max, a['max']);
                                       sum += a['sum'];
                                       count += a['count'];
                                   }
                                   double avg = count > 0 ? sum / count : 0;
                                   return [ 'min': min, 
                                            'max': max, 
                                            'avg': avg ];";
}
