# Exercise 3: Practicing Helm Commands with a Sample Application

In this exercise, you will learn how to install, upgrade, and manage applications in Kubernetes using Helm.

## Pre-requisites

- [Helm](https://helm.sh/docs/intro/install/) installed  
- [kubectl](https://kubernetes.io/docs/tasks/tools/) installed and configured  
- A running Kubernetes cluster (Docker Desktop, Minikube, k3s, or kind)  

---

## Instructions

### 1. Add a Helm Repository

Add the official Bitnami Helm repository:

~~~bash
helm repo add bitnami https://charts.bitnami.com/bitnami
~~~

Update your local repo cache:

~~~bash
helm repo update
~~~

List available charts from the repo:

~~~bash
helm search repo bitnami
~~~

---

### 2. Install a Helm Chart

Install the `nginx` chart in a namespace called `demo`:

~~~bash
kubectl create namespace demo
helm install my-nginx bitnami/nginx --namespace demo
~~~

Verify the release:

~~~bash
helm list -n demo
kubectl get pods -n demo
~~~

---

### 3. Inspect and Upgrade a Release

View the details of your Helm release:

~~~bash
helm status my-nginx -n demo
~~~

Upgrade the release to change a configuration (for example, replica count):

~~~bash
helm upgrade my-nginx bitnami/nginx --namespace demo --set replicaCount=2
~~~

Verify the upgrade:

~~~bash
kubectl get pods -n demo
helm history my-nginx -n demo
~~~

---

### 4. Rollback a Release

If something goes wrong, rollback to the previous revision:

~~~bash
helm rollback my-nginx 1 -n demo
~~~

Check that the rollback was successful:

~~~bash
helm history my-nginx -n demo
kubectl get pods -n demo
~~~

---

### 5. Uninstall a Release

Remove the Helm release and clean up resources:

~~~bash
helm uninstall my-nginx -n demo
kubectl get pods -n demo
~~~

---

### Optional Exercises

- Install **Postgres** using Helm and customize the username, password, and database name with `--set`.  
- Explore the default `values.yaml` for a chart and override multiple values during installation.  
- Try installing multiple releases of the same chart in the same cluster using different namespaces.  
- Use `helm template` to render Kubernetes manifests without applying them.  
- Inspect and export the manifest for a release using:

~~~bash
helm get manifest my-nginx -n demo
~~~

---

## Summary

In this exercise, you learned how to:

- Add and update Helm repositories.  
- Install applications using Helm charts.  
- Inspect, upgrade, and rollback Helm releases.  
- Uninstall releases and clean up resources.  
- Customize charts using `--set` and explore values.  

For more information:

- [Helm Documentation](https://helm.sh/docs/)  
- [Bitnami Helm Charts](https://bitnami.com/stacks/helm)
