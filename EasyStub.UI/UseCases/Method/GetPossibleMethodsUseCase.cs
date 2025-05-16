namespace EasyStub.UI.UseCases.Method;

public class GetPossibleMethodsUseCase : IUseCase
{

    public class GetPossibleMethodsUseCaseException : Exception
    {
        public GetPossibleMethodsUseCaseException() : base($"{nameof(GetPossibleMethodsUseCaseException)} got an error")
        { }
    }

    public List<HttpMethod> Handle() => typeof(HttpMethod)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(HttpMethod))
            .Select(x => (HttpMethod?)x.GetValue(null))
            .Select(x => x ?? throw new GetPossibleMethodsUseCaseException())
            .ToList();
}