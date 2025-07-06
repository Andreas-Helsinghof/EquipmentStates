FROM python:3.11

# Set work directory
WORKDIR /app

# Copy server code
COPY ./ opcua_python_server/

# Install Python dependencies
RUN pip install --no-cache-dir asyncua

# Expose OPC UA port
EXPOSE 4840

# Run the OPC UA server
CMD ["python", "./opcua_python_server/server.py"]
