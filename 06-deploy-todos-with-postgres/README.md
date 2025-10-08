# Exercise 5: Build, Push, and Deploy Todos API with Postgres

In this exercise, you will build a Docker image for the `Todos.Api`, push it to AWS ECR, deploy Postgres, deploy the API on Kubernetes, and interact with the API using `curl`.

## Pre-requisites

- [Docker](https://docs.docker.com/get-docker/)  
- [kubectl](https://kubernetes.io/docs/tasks/tools/) installed and configured  
- AWS CLI configured with permissions to ECR  
- A running Kubernetes cluster (Docker Desktop, Minikube, k3s, or kind)  

## Instructions

### 1. Build the Docker image

Navigate to the `Todos.Api` folder:

~~~bash
cd 06-deploy-todos-with-postgres/Todos.Api
~~~

Build the Docker image:

~~~bash
docker build -t todos-api .
~~~

List local Docker images to verify:

~~~bash
docker images
~~~

### 2. Push the image to AWS ECR

1. Create a repository in ECR (if not already created):

~~~bash
aws ecr create-repository --repository-name todos-api
~~~

2. Authenticate Docker with ECR:

~~~bash
aws ecr get-login-password --region <your-region> | docker login --username AWS --password-stdin <your-account-id>.dkr.ecr.<your-region>.amazonaws.com
~~~

3. Tag the Docker image for ECR:

~~~bash
docker tag todos-api:latest <your-account-id>.dkr.ecr.<your-region>.amazonaws.com/todos-api:latest
~~~

4. Push the image to ECR:

~~~bash
docker push <your-account-id>.dkr.ecr.<your-region>.amazonaws.com/todos-api:latest
~~~

### 3. Deploy Postgres

Apply the provided Kubernetes manifest:

~~~bash
kubectl apply -f todos-with-postgres/k8s-manifests/postgres.yaml
~~~

Verify the Pod is running:

~~~bash
kubectl get pods
~~~

### 4. Deploy the Todos API

Create a Deployment YAML (`todos-api-deploy.yaml`) like this:

~~~yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: todos-api
spec:
  replicas: 1
  selector:
    matchLabels:
      app: todos-api
  template:
    metadata:
      labels:
        app: todos-api
    spec:
      containers:
        - name: todos-api
          image: <your-account-id>.dkr.ecr.<your-region>.amazonaws.com/todos-api:latest
          ports:
            - containerPort: 8080
          env:
            - name: POSTGRES_HOST
              value: postgres
            - name: POSTGRES_USER
              value: postgres
            - name: POSTGRES_PASSWORD
              value: postgres
            - name: POSTGRES_DB
              value: todos
---
apiVersion: v1
kind: Service
metadata:
  name: todos-api
spec:
  type: NodePort
  selector:
    app: todos-api
  ports:
    - port: 8080
      targetPort: 8080
~~~

Apply the Deployment:

~~~bash
kubectl apply -f todos-api-deploy.yaml
~~~

Check that the Pod is running:

~~~bash
kubectl get pods
kubectl get svc
~~~

> Note the NodePort assigned to the service. You can access the API at `http://<node-ip>:<node-port>`.

### 5. Test the Todos API

Use `curl` to add a new task:

~~~bash
curl -X POST -H "Content-Type: application/json" \
-d '{"description": "Learn Kubernetes", "completed": false}' \
http://localhost:8080/api/tasks
~~~

Check that the task was added by retrieving all tasks:

~~~bash
curl http://localhost:8080/api/tasks
~~~

---

## Optional Exercises

- Scale the API Deployment to multiple replicas:

~~~bash
kubectl scale deployment todos-api --replicas=3
~~~

- Update the API image and do a rolling update.  
- Deploy a LoadBalancer service if using a cloud provider.  
- Explore the logs of the API Pod:

~~~bash
kubectl logs <todos-api-pod-name>
~~~

- Try deleting the Postgres Pod and see how Kubernetes recovers it.  

---

## Summary

In this exercise, you learned how to:

- Build a Docker image and push it to AWS ECR.  
- Deploy a database using a Kubernetes manifest.  
- Deploy an application connecting to Postgres.  
- Access and test the application API using `curl`.  
- Use Kubernetes Deployment and Service resources to manage an app.  

For more information:

- [Kubernetes Deployments](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/)  
- [Kubernetes Services](https://kubernetes.io/docs/concepts/services-networking/service/)  
- [AWS ECR Documentation](https://docs.aws.amazon.com/AmazonECR/latest/userguide/what-is-ecr.html)
