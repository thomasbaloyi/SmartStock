To run the app:

	1. Ensure that the docker engine is installed and running on host machine: https://docs.docker.com/engine/install/
	2. Navigate to the root SmartStock directory and build the image: docker build . -t smartstock-app
	3. Run the containers: docker run -d -p 8080:80 --name smartstock-container smartstock-app
	4. The application should now be accessible on http://localhost:8080


Clean-up after testing:
	1. Run: docker stop smartstock-container
	2. Run: docker rm smartstock-container
	3. Run: docker rmi smartstock-app
