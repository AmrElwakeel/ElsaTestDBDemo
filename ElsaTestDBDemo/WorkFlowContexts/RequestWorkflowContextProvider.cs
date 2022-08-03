using Elsa.Activities.Http.Models;
using Elsa.Services;
using Elsa.Services.Models;
using ElsaTestDBDemo.DomainDatabase;
using ElsaTestDBDemo.DomainDatabase.Entites;

namespace ElsaTestDBDemo.WorkFlowContexts
{
    public class RequestWorkflowContextProvider : WorkflowContextRefresher<Request>
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public RequestWorkflowContextProvider(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        /// <summary>
        /// Loads a BlogPost entity from the database.
        /// </summary>
        public override async ValueTask<Request> LoadAsync(LoadWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var blogPostId = context.ContextId;
            return _applicationDBContext.Requests.AsQueryable().FirstOrDefault(x => x.Id == blogPostId);
        }

        /// <summary>
        /// Updates a BlogPost entity in the database.
        /// If there's no actual workflow context, we will get it from the input. This works because we know we have a workflow that starts with an HTTP Endpoint activity that receives BlogPost models.
        /// This is a design choice for this particular demo. In real world scenarios, you might not even need this since your workflows may be dealing with existing entities, or have (other) workflows that handle initial entity creation.
        /// The key take away is: you can do whatever you want with these workflow context providers :) 
        /// </summary>
        public override async ValueTask<string> SaveAsync(SaveWorkflowContext<Request> context, CancellationToken cancellationToken = default)
        {
            var request = context.Context;
            var dbSet = _applicationDBContext.Requests;

            if (request == null)
            {
                // We are handling a newly posted blog post.
                request = ((HttpRequestModel)context.WorkflowExecutionContext.Input!).GetBody<Request>();

                if (request == null)
                    request = new Request();

                // Generate a new ID.
                request.Id = Guid.NewGuid().ToString("N");

                // Set IsPublished to false to prevent caller from cheating ;)


                // Set context.
                context.WorkflowExecutionContext.WorkflowContext = request;
                context.WorkflowExecutionContext.ContextId = request.Id;

                // Add blog post to DB.
                await dbSet.AddAsync(request, cancellationToken);
            }
            else
            {
                var requestId = request.Id;
                var existingRequest = dbSet.AsQueryable().Where(x => x.Id == requestId).First();

                request.ExecutionDate=DateTime.Now;

                _applicationDBContext.Entry(existingRequest).CurrentValues.SetValues(request);
            }

            await _applicationDBContext.SaveChangesAsync(cancellationToken);
            return request.Id;
        }
    }
}
