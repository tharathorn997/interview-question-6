# Product Code Management System (IT 06-1)

This project is a full-stack product code management system, generated as an example of a CRUD application meeting specific exam requirements.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | C# .NET Core 8 Web API |
| Frontend | Angular 17+ (Standalone Components) |
| Database | SQL Server 2022 |
| Container | Docker + Docker Compose |
| Orchestration | Kubernetes |

## Prerequisites

- Docker Desktop (or equivalent)
- Kubernetes cluster (optional, for K8s deployment)

## Running the Application Locally

1. Build and start the services using Docker Compose:
   ```bash
   docker-compose up --build -d
   ```

2. Access the frontend application at [http://localhost:4200](http://localhost:4200).

3. The backend API is available at [http://localhost:5000](http://localhost:5000).

## Kubernetes Deployment

Deploy the system to a Kubernetes cluster using the provided manifests:

```bash
kubectl apply -f k8s/
```
