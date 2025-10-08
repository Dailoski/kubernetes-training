# Exercise 2.1: Practicing `kubectl apply` with Manifests

In this exercise, you will create and apply Kubernetes manifests using `kubectl apply`, explore resource updates, and practice managing resources declaratively.

## Pre-requisites

- [kubectl](https://kubernetes.io/docs/tasks/tools/) installed and configured  
- A running Kubernetes cluster (Docker Desktop, Minikube, k3s, or kind)  

---

## Instructions

### 1. Create a Deployment manifest

Create a file called `nginx-deployment.yaml`:

~~~yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: nginx-deploy
spec:
  replicas: 2
  selector:
    matchLabels:
      app: nginx
  template:
    metadata:
      labels:
        app: nginx
    spec:
      containers:
        - name: nginx
          image: nginx:alpine
          ports:
            - containerPort: 80
~~~

---

### 2. Apply the manifest

Apply the Deployment to the cluster:

~~~bash
kubectl apply -f nginx-deployment.yaml
kubectl get deployments
kubectl get pods
~~~

---

### 3. Create a Service manifest

Create a file called `nginx-service.yaml`:

~~~yaml
apiVersion: v1
kind: Service
metadata:
  name: nginx-service
spec:
  selector:
    app: nginx
  type: NodePort
  ports:
    - port: 80
      targetPort: 80
~~~

Apply the Service:

~~~bash
kubectl apply -f nginx-service.yaml
kubectl get svc nginx-service
~~~

> Note the `NodePort`. You can access the app at `http://<node-ip>:<node-port>`.

---

### 4. Update the Deployment manifest

Edit the Deployment YAML to change replicas or container image version. For example:

~~~yaml
spec:
  replicas: 3
  containers:
    - name: nginx
      image: nginx:1.23-alpine
~~~

Apply the changes:

~~~bash
kubectl apply -f nginx-deployment.yaml
kubectl get deployments
kubectl get pods
~~~

---

### 5. Delete resources with `kubectl apply`

~~~bash
kubectl delete -f nginx-service.yaml
kubectl delete -f nginx-deployment.yaml
~~~

---

### Optional Exercises

- Combine Deployment and Service into **a single manifest file** and apply together.  
- Use `kubectl apply -f .` to apply all manifests in a directory.  
- Explore `kubectl diff -f nginx-deployment.yaml` to see what changes would be applied.  
- Try updating container environment variables or adding labels, then re-apply.  
- Use `kubectl rollout status deployment nginx-deploy` to watch rollout progress.  

---

## Summary

In this exercise, you learned how to:

- Create Kubernetes manifests for Deployments and Services.  
- Apply manifests using `kubectl apply`.  
- Update resources declaratively and verify changes.  
- Clean up resources using manifests.  

For more information:

- [Kubernetes `kubectl apply`](https://kubernetes.io/docs/reference/generated/kubectl/kubectl-commands#apply)  
- [Kubernetes Deployments](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/)  
- [Kubernetes Services](https://kubernetes.io/docs/concepts/services-networking/service/)
