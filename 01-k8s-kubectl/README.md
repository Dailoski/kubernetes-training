# Exercise 1: Run a Pod from an Image on Docker Hub

In this exercise, we will run a Kubernetes Pod from an existing image hosted on Docker Hub.

## Pre-requisites

- [kubectl](https://kubernetes.io/docs/tasks/tools/) installed and configured  
- A running Kubernetes cluster (Docker Desktop, Minikube, k3s, or kind)

## Instructions

1. Create a Pod using the `nginx` image:

~~~bash
kubectl run my-nginx --image=nginx --port=80
~~~

> [!NOTE] `kubectl run` creates a single Pod. The `--port` flag indicates the container port exposed. Kubernetes will pull the image if it is not available locally.

2. List running Pods:

~~~bash
kubectl get pods
~~~

3. View detailed information about the Pod:

~~~bash
kubectl describe pod my-nginx
~~~

4. View logs of the Pod:

~~~bash
kubectl logs my-nginx
~~~

5. Expose the Pod as a service so you can access it from your host:

~~~bash
kubectl expose pod my-nginx --type=NodePort --port=80
~~~

6. Get the service details to find the port:

~~~bash
kubectl get svc
~~~

> Note the `NodePort` assigned. You can access the app using `http://<node-ip>:<node-port>`.

7. Delete the service:

~~~bash
kubectl delete svc my-nginx
~~~

8. Delete the Pod:

~~~bash
kubectl delete pod my-nginx
~~~

---

### Optional Exercises

- Run the Pod with a specific version of Nginx, e.g., `nginx:1.23-alpine`.  
- Run multiple replicas of Nginx using a Deployment:

~~~bash
kubectl create deployment my-nginx-deploy --image=nginx --replicas=3
~~~

- Scale the Deployment up or down:

~~~bash
kubectl scale deployment my-nginx-deploy --replicas=5
~~~

- Delete the Deployment:

~~~bash
kubectl delete deployment my-nginx-deploy
~~~

- Explore running a different image (e.g., `httpd`, `redis`) in a Pod.  

---

## Summary

In this exercise, you learned how to:  
- Run a Pod using a Docker Hub image.  
- View Pod status and logs.  
- Expose a Pod via a Service.  
- Create and manage Deployments for multiple replicas.  

For more information, refer to the [kubectl run documentation](https://kubernetes.io/docs/reference/kubectl/overview/).  
